using Core.Triggers;
using Icons;
using Level;
using UnityEngine;

namespace Creatures.Behaviour
{
    public class BreedBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => 5;

        [SerializeField]
        private BaseCreature _breedingMate;

        public override BehaviourState StateSuggestion => BehaviourState.Breeding;

        protected enum BreedState { Cooldown, Idle, Breeding, };

        [SerializeField]
        private BreedState breedState = BreedState.Cooldown;

        public override bool IsEligibleForActivation => false;

        private float _durationTimer = 0;

        private CircleTrigger _startBreedTrigger;

        public void Start()
        {
            _startBreedTrigger = CircleTrigger.Create(_creatureData.StartBreedRange, "StartBreedRange", transform);
            _startBreedTrigger.OnTriggerEnter += CheckForBreedingMateFound;
            _startBreedTrigger.OnTriggerExit += CheckForBreedingMateLost;

            GoInCoolDown();
            _creature.Reproduce = _creatureData.StartReproduce;
        }

        protected override void UpdateWhenActive()
        {
            if (_creature.Reproduce < _creatureData.StartReproduce)
            {
                OnDeactivationRequest?.Invoke(this);
                return;
            }
            _durationTimer = Mathf.Clamp(_durationTimer - Time.deltaTime, 0, _durationTimer);
            if (_durationTimer <= 0 && _creature.Alive)
            {
                if (breedState == BreedState.Breeding)
                {
                    Hatch();
                }
                if (breedState == BreedState.Cooldown)
                {
                    breedState = BreedState.Idle;
                }
            }
        }

        protected override void UpdateWhenInactive()
        {
            _durationTimer = Mathf.Clamp(_durationTimer - Time.deltaTime, 0, _durationTimer);
            if (breedState == BreedState.Cooldown && _durationTimer <= 0)
            {
                breedState = BreedState.Idle;
            }

            if (breedState == BreedState.Idle && _breedingMate != null && CanReproduce())
            {
                OnActivationRequest?.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _durationTimer = _creatureData.BreedingTime;
                breedState = BreedState.Breeding;
                _creature.Movement.Stop();
                
                _startBreedTrigger.OnTriggerEnter -= CheckForBreedingMateFound;
                _startBreedTrigger.OnTriggerExit -= CheckForBreedingMateLost;
            }
            else
            {
                _startBreedTrigger.OnTriggerEnter += CheckForBreedingMateFound;
                _startBreedTrigger.OnTriggerExit += CheckForBreedingMateLost;
            }
        }

        private void Hatch()
        {
            if(_creature.Reproduce >= _creatureData.CanReproduceThreshold && _breedingMate.Reproduce >= _breedingMate.creatureData.CanReproduceThreshold)
            {
                Vector2 parentPosition = new Vector2(transform.position.x, transform.position.y);
                Vector2 rndPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                Vector2Int gridPos = LevelManager.Instance.Grid.WorldToGridPosition(parentPosition + rndPosition);
                int tries = 0;
                while (LevelManager.Instance.Grid.InBounds(gridPos) == false ||
                       LevelManager.Instance.Grid[gridPos.x, gridPos.y].Walkable == false)
                {
                    rndPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    gridPos = LevelManager.Instance.Grid.WorldToGridPosition(parentPosition + rndPosition);
                    tries++;
                }
                
                IconManager.Instance.Create("Newborn", parentPosition + Vector2.up*.2f);
                
                var junior = Instantiate(_creatureData.Prefab, parentPosition+rndPosition, Quaternion.identity);
                junior.name = $"{gameObject.name}";
                _creature.PlayBreedSound();
                _creature.Hunger += 20;
                _breedingMate.Hunger += 20;
                _creature.Reproduce = 0;
                _breedingMate.Reproduce = 0;
            }
            GoInCoolDown();
        }

        private void GoInCoolDown()
        {
            breedState = BreedState.Cooldown;
            _durationTimer = _creatureData.BreedingCooldown;
            OnDeactivationRequest.Invoke(this);
        }

        private void CheckForBreedingMateFound(Collider2D collider)
        {
            if (collider.isTrigger == false && _breedingMate == null)
            {
                var breedingTarget = collider.gameObject.GetComponent<BaseCreature>();
                if (breedingTarget == null)
                {
                    return;
                }
                
                if (breedingTarget.GetType() == _creature.GetType())
                {
                    bool iCanReproduce = _creature.Reproduce >= _creatureData.CanReproduceThreshold;
                    bool targetCanReproduce = breedingTarget.Reproduce >= breedingTarget.creatureData.CanReproduceThreshold;
                    if (iCanReproduce && targetCanReproduce)
                    {
                        _breedingMate = breedingTarget;
                    }
                }
            }
        }

        private void CheckForBreedingMateLost(Collider2D collider)
        {
            if (collider.isTrigger == false && _breedingMate != null)
            {
                var leavingCreature = collider.gameObject.GetComponent<BaseCreature>();
                if (leavingCreature == null)
                {
                    return;
                }

                if (leavingCreature.GetType() == _breedingMate.GetType())
                {
                    _breedingMate = null;
                }
            }
        }

        private bool CanReproduce()
        {
            return _creature.Reproduce >= _creatureData.CanReproduceThreshold;
        }
    }
}