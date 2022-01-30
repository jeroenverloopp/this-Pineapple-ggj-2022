﻿using UnityEngine;

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
            if (active == Active)
            {
                return;
            }
            base.SetActive(active);
            if (Active)
            {
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