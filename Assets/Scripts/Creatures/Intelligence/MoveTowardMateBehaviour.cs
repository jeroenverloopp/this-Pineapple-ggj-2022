using UnityEngine;

namespace Creatures.Behaviour
{
    public class MoveTowardMateBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => Mathf.RoundToInt(_creature.Reproduce / 25);

        private GameObject targetMate;

        public override BehaviourState StateSuggestion => BehaviourState.FindingMate;

        public override bool IsEligibleForActivation => targetMate != null && _creature.Reproduce > 40;

        private void Start()
        {
            if (_creature != null)
            {
                _creature.sightCollider.OnTriggerEnter2DAction.AddListener(CheckForMateEnterSight);
                _creature.sightCollider.OnTriggerExit2DAction.AddListener(CheckForMateLeaveSight);
            }
        }

        protected override void UpdateWhenActive()
        {
            if (targetMate == null)
            {
                // TargetMate is destroyed
                OnDeactivationRequest.Invoke(this);
            }
        }

        protected override void UpdateWhenInactive()
        {
            if (IsEligibleForActivation)
            {
                OnActivationRequest.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _creature.sightCollider.OnTriggerEnter2DAction.RemoveListener(CheckForMateEnterSight);
                _creature.sightCollider.OnTriggerExit2DAction.RemoveListener(CheckForMateLeaveSight);

                _creature.Movement.OnTargetReached += OnTargetReached;
                _creature.Movement.OnSetTargetFailed += MoveTowardsTarget;

                MoveTowardsTarget();
            }
            else
            {
                _creature.sightCollider.OnTriggerEnter2DAction.AddListener(CheckForMateEnterSight);
                _creature.sightCollider.OnTriggerExit2DAction.AddListener(CheckForMateLeaveSight);

                _creature.Movement.OnTargetReached -= OnTargetReached;
                _creature.Movement.OnSetTargetFailed -= MoveTowardsTarget;

                _creature.Movement.Stop();
            }
        }

        private void CheckForMateEnterSight(Collider2D collider)
        {
            if (!collider.isTrigger)
            {
                if (collider.gameObject.tag.Equals(gameObject.tag) && !Active)
                {
                    Debug.Log("Found Mate");
                    targetMate = collider.gameObject;
                }
            }
        }

        private void CheckForMateLeaveSight(Collider2D collider)
        {
            if (!collider.isTrigger && targetMate != null)
            {
                if (collider.gameObject.Equals(targetMate) && !Active)
                {
                    targetMate = null;
                }
            }
        }

        private void MoveTowardsTarget()
        {
            if (targetMate != null)
            {
                _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
                _creature.Movement.SetTarget(targetMate.transform.position);
            }
            else
            {
                OnDeactivationRequest.Invoke(this);
            }
        }

        private void OnTargetReached()
        {
            OnDeactivationRequest.Invoke(this);
        }
    }
}