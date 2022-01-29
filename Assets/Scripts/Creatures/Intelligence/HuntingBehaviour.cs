using System;
using Core.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Behaviour
{
    public class HuntingBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority
        {
            get
            {
                float diff = _creature.Hunger - _creatureData.HuntHunger;
                return (int)Mathf.Max(diff * _creatureData.HuntingPriorityIncrease, 0);
            }
        }
        
        public override BehaviourState StateSuggestion => BehaviourState.LookingForFood;
        
        public override bool IsEligibleForActivation => false;
        
        
        private float _durationTimer = 0;


        private CircleTrigger _visionTrigger;
        private CircleTrigger _eatingTrigger;
        
        public override void Set(BaseCreatureData creatureData, BaseCreature creature)
        {
            base.Set(creatureData, creature);
            
            _visionTrigger = CircleTrigger.Create(creatureData.FindFoodRange, "FindFoodRange", transform);
            _eatingTrigger = CircleTrigger.Create(creatureData.EatRange, "EatRange", transform);
            _visionTrigger.OnTrigger += OnFoundFood;
            _eatingTrigger.OnTrigger += OnEatFood;
        }

        private void OnDestroy()
        {
            _visionTrigger.OnTrigger -= OnFoundFood;
            _eatingTrigger.OnTrigger -= OnEatFood;
        }

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
            if (_creature.Hunger > _creatureData.HuntHunger)
            {
                OnActivationRequest?.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (Active)
            {
                
                _durationTimer = Random.Range(1000,10000);
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
        
        private void OnFoundFood(Collider2D collider)
        {
            
            var creature = collider.gameObject.GetComponent<BaseCreature>();
            if (creature == null)
            {
                return;
            }
            
            if (_creatureData.EatsCreatures.Contains(creature.creatureData))
            {
                Debug.Log("Found food!");
            }
        }
        
        private void OnEatFood(Collider2D collider)
        {
            Debug.Log("Eat food!");
        }


        private void FindHuntTarget()
        {
            Collider2D[] results = new Collider2D[1];
            Physics2D.OverlapCircle(transform.position, _creatureData.FindFoodRange);
        }

        private void FindRoamTarget()
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