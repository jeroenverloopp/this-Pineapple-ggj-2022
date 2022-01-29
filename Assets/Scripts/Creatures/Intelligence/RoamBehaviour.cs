using UnityEngine;

namespace Creatures.Behaviour
{
    public class RoamBehaviour : BaseBehaviour
    {
        public override int Priority => 1;
        
        public override BehaviourState StageSuggestion => BehaviourState.Roaming;
        
        public override bool IsEligibleForActivation => true;
        
        protected override void UpdateWhenActive()
        {
        }

        protected override void UpdateWhenInactive()
        {
            
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                SetNewTarget();
                _creature.Movement.OnTargetReached += SetNewTarget;
                _creature.Movement.OnSetTargetFailed += SetNewTarget;
            }
            else
            {
                _creature.Movement.OnTargetReached -= SetNewTarget;
                _creature.Movement.OnSetTargetFailed -= SetNewTarget;
            }
        }

        private void SetNewTarget()
        {
            _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
            _creature.Movement.SetTarget(new Vector2(Random.Range(-50,50),Random.Range(-50,50)));
        }
    }
}