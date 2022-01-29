using UnityEngine;

namespace Creatures.Behaviour
{
    public class BreedBehaviour : BaseBehaviour
    {
        public override int Priority => 5;

        private bool mateInReach = false;

        public override BehaviourState StateSuggestion => BehaviourState.Breeding;

        protected enum BreedState { Breeding, Cooldown, Idle };

        private BreedState breedState = BreedState.Cooldown;

        public override bool IsEligibleForActivation => breedState == BreedState.Idle && mateInReach && _creatureData.CanBreed;
        
        private float _durationTimer = 0;
        
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
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _durationTimer = _creatureData.BreedingTime;
                breedState = BreedState.Breeding;

                _creature.reachCollider.OnTriggerEnter2DAction.AddListener(CheckForMateInReach);
                _creature.reachCollider.OnTriggerExit2DAction.AddListener(CheckForMateLeaveReach);
            }
            else
            {
                GoInCoolDown();
                _creature.reachCollider.OnTriggerEnter2DAction.RemoveListener(CheckForMateInReach);
                _creature.reachCollider.OnTriggerExit2DAction.RemoveListener(CheckForMateLeaveReach);
            }
        }

        private void Hatch()
        {
            var junior = Instantiate(_creatureData.Prefab, (new Vector2(transform.position.x, transform.position.y) + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))), Quaternion.identity);
            GoInCoolDown();
        }

        private void GoInCoolDown()
        {
            breedState = BreedState.Cooldown;
            _durationTimer = _creatureData.BreedingCooldown;
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