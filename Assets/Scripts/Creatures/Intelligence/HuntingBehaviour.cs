using System;
using Core.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Behaviour
{
    public class HuntingBehaviour : BaseBehaviour
    {
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

        private BaseCreature _huntingTarget;
        
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
            if (_creature.Hunger < _creatureData.HuntHunger)
            {
                OnDeactivationRequest?.Invoke(this);
                return;
            }

            if (_huntingTarget != null)
            {
                if (Vector2.Distance(_huntingTarget.transform.position, transform.position) >
                    _creatureData.FindFoodRange + 10)
                {
                    SetHuntingTarget(null);
                    return;
                }

                if (_creature.Movement.Moving)
                {
                    float distFromPathToTarget = Vector2.Distance(_huntingTarget.transform.position, _creature.Movement.TargetPosition);
                    if (distFromPathToTarget > _creatureData.EatRange-0.5f)
                    {
                        SetHuntingTarget(_huntingTarget);
                    }
                }
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
                _creature.Movement.OnTargetReached += OnTargetReached;
                _creature.Movement.OnSetTargetFailed += OnTargetReached;
            }
            else
            {
                _creature.Movement.OnTargetReached -= OnTargetReached;
                _creature.Movement.OnSetTargetFailed -= OnTargetReached;
                _huntingTarget = null;
                _creature.Movement.Stop();
            }
        }
        
        private void OnFoundFood(Collider2D collider)
        {
            if (Active == false)
            {
                return;
            }
            
            var creature = collider.gameObject.GetComponent<BaseCreature>();
            if (creature == null)
            {
                return;
            }
            
            if (_creatureData.EatsCreatures.Contains(creature.creatureData))
            {
                if (_huntingTarget == null)
                {
                    SetHuntingTarget(creature);
                }
                else
                {
                    float newDist = Vector2.Distance(transform.position, creature.transform.position);
                    float currentDist = Vector2.Distance(transform.position, _huntingTarget.transform.position);
                    if (newDist < currentDist)
                    {
                        SetHuntingTarget(creature);
                    }
                }
            }
        }

        private void OnEatFood(Collider2D collider)
        {
            if (Active == false)
            {
                return;
            }
            
            var creature = collider.gameObject.GetComponent<BaseCreature>();
            if (creature == null)
            {
                return;
            }

            if (_creatureData.EatsCreatures.Contains(creature.creatureData))
            {
                _creature.Eat(creature);
            }

            _creature.Movement.Stop();
            SetHuntingTarget(null);
        }

        private void SetHuntingTarget(BaseCreature creature)
        {
            _huntingTarget = creature;
            if (_huntingTarget != null)
            {
                _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
                _creature.Movement.SetTarget(_huntingTarget.transform);
            }
            else
            {
                SetRoamTarget();
            }
        }

        private void OnTargetReached()
        {
            if (_huntingTarget == null)
            {
                SetRoamTarget();
            }
        }

        private void SetRoamTarget()
        {
            _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
            Vector2 currentPosition = _creature.transform.position;
            float range = _creatureData.RoamRange;
            Vector2 offset = new Vector2(Random.Range(-range,range),Random.Range(-range,range));
            _creature.Movement.SetTarget(currentPosition + offset);
        }
    }
}