using UnityEngine;

namespace Creatures.Behaviour
{
    public class IdleBehaviour : BaseBehaviour
    {
        public override int Priority => 1;
        
        public override BehaviourState StageSuggestion => BehaviourState.Idle;
        
        public override bool IsEligibleForActivation => true;


        private float _durationTimer = 0;
        
        protected override void UpdateWhenActive()
        {
            _durationTimer = Mathf.Clamp(_durationTimer - Time.deltaTime, 0, _durationTimer);
            if (_durationTimer <= 0)
            {
                OnDeactivationRequest?.Invoke(this);
            }
        }

        protected override void UpdateWhenInactive()
        {
            
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _creature.Movement.Stop();
                _durationTimer = Random.Range(_creatureData.MinIdleTime, _creatureData.MaxIdleTime);
            }
        }

    }
}