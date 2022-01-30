using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Behaviour
{
    public class RoamingBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => 1;
        
        public override BehaviourState StateSuggestion => BehaviourState.Roaming;
        
        public override bool IsEligibleForActivation => true;
        
        [SerializeField]
        private float _durationTimer = 0;

        private void OnDestroy()
        {
            _creature.Movement.OnTargetReached -= SetNewTarget;
            _creature.Movement.OnSetTargetFailed -= SetNewTarget;
        }

        protected override void UpdateWhenActive()
        {
            _durationTimer = Mathf.Clamp(_durationTimer - Time.deltaTime, 0, _durationTimer);
            if (_durationTimer <= 0 || _creature.Alive == false)
            {
                OnDeactivationRequest?.Invoke(this);
            }
        }

        protected override void UpdateWhenInactive()
        {
            
        }

        public override void SetActive(bool active)
        {
            if (active == Active)
            {
                return;
            }
            base.SetActive(active);
            if (Active)
            {
                Debug.Log("Activate");
                _durationTimer = Random.Range(_creatureData.MinRoamingTime, _creatureData.MaxRoamingTime);
                _creature.Movement.OnTargetReached += SetNewTarget;
                _creature.Movement.OnSetTargetFailed += SetNewTarget;
                SetNewTarget();
            }
            else
            {
                _creature.Movement.OnTargetReached -= SetNewTarget;
                _creature.Movement.OnSetTargetFailed -= SetNewTarget;
                
            }
        }

        private void DoNothing()
        {
            
        }

        private void SetNewTarget()
        {
            _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
            Vector2 currentPosition = _creature.transform.position;
            float range = _creatureData.RoamRange;
            Vector2 offset = new Vector2(Random.Range(-range,range),Random.Range(-range,range));
            _creature.Movement.SetTarget(currentPosition + offset);
        }
    }
}