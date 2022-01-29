using UnityEngine;

namespace Creatures.Behaviour
{
    public class BreedBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => 5;

        private bool mateInReach = false;

        public override BehaviourState StateSuggestion => BehaviourState.Breeding;

        protected enum BreedState { Cooldown, Idle, Breeding, };

        [SerializeField]
        private BreedState breedState = BreedState.Cooldown;

        public override bool IsEligibleForActivation
        {
            get
            {
                return breedState == BreedState.Idle && mateInReach && _creatureData.CanBreed && _creature.Reproduce > 50;
            }
        }
        
        private float _durationTimer = 0;

        public void Start()
        {
            _creature.reachCollider.OnTriggerEnter2DAction.AddListener(CheckForMateInReach);
            _creature.reachCollider.OnTriggerExit2DAction.AddListener(CheckForMateLeaveReach);

            GoInCoolDown();
            _creature.Reproduce = _creatureData.StartReproduce;
        }

        protected override void UpdateWhenActive()
        {
            _durationTimer = Mathf.Clamp(_durationTimer - Time.deltaTime, 0, _durationTimer);
            if (_durationTimer <= 0)
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

            if (IsEligibleForActivation)
            {
                Debug.Log($"{transform.name} requests breeding with priority {Priority}");
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

                _creature.reachCollider.OnTriggerEnter2DAction.RemoveListener(CheckForMateInReach);
                _creature.reachCollider.OnTriggerExit2DAction.RemoveListener(CheckForMateLeaveReach);
            }
            else
            {
                _creature.reachCollider.OnTriggerEnter2DAction.AddListener(CheckForMateInReach);
                _creature.reachCollider.OnTriggerExit2DAction.AddListener(CheckForMateLeaveReach);
            }
        }

        private void Hatch()
        {
            var junior = Instantiate(_creatureData.Prefab, (new Vector2(transform.position.x, transform.position.y) + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))), Quaternion.identity);
            junior.name = $"Junior {(int)Time.realtimeSinceStartup}";
            GoInCoolDown();
        }

        private void GoInCoolDown()
        {
            _creature.Reproduce = 0;
            breedState = BreedState.Cooldown;
            _durationTimer = _creatureData.BreedingCooldown;

            OnDeactivationRequest.Invoke(this);
        }

        private void CheckForMateInReach(Collider2D collider)
        {
            if (!collider.isTrigger)
            {
                if (collider.gameObject.tag.Equals(gameObject.tag))
                {
                    mateInReach = true;
                }
            }
        }

        private void CheckForMateLeaveReach(Collider2D collider)
        {
            if (!collider.isTrigger)
            {
                if (collider.gameObject.tag.Equals(gameObject.tag))
                {
                    mateInReach = false;
                }
            }
        }
    }
}