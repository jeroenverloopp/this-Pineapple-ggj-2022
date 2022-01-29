using UnityEngine;

namespace Creatures.Behaviour
{
    public class MoveTowardMateBehaviour : BaseBehaviour
    {
        public override int Priority => 3;

        private GameObject targetMate;

        public override BehaviourState StateSuggestion => BehaviourState.FindingMate;

        public override bool IsEligibleForActivation => targetMate != null;
        
        private float _durationTimer = 0;
        
        protected override void UpdateWhenActive()
        {

        }

        protected override void UpdateWhenInactive()
        {
            _durationTimer = Mathf.Clamp(_durationTimer - Time.deltaTime, 0, _durationTimer);

            if (targetMate)
            {
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _creature.sightCollider.OnTriggerEnter2DAction.AddListener(CheckForMateEnterSight);
                _creature.sightCollider.OnTriggerEnter2DAction.AddListener(CheckForMateLeaveSight);
            }
            else
            {
                _creature.sightCollider.OnTriggerEnter2DAction.RemoveListener(CheckForMateEnterSight);
                _creature.sightCollider.OnTriggerEnter2DAction.RemoveListener(CheckForMateLeaveSight);
            }
        }

        private void CheckForMateEnterSight(Collider2D collider)
        {
            if (!collider.isTrigger)
            {
                if (collider.gameObject.tag.Equals(gameObject.tag) && targetMate != null)
                {
                    targetMate = collider.gameObject;
                }
            }
        }

        private void CheckForMateLeaveSight(Collider2D collider)
        {
            if (!collider.isTrigger)
            {
                if (collider.gameObject.Equals(targetMate))
                {
                    targetMate = null;
                }
            }
        }
    }
}