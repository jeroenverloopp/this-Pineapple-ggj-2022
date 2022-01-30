using System;
using Core.Triggers;
using Food;
using Level;
using UnityEngine;

namespace Creatures.Behaviour
{
    public class MoveTowardsFoodBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => Mathf.RoundToInt(_creature.Hunger-_creatureData.StartForagingThreshold * _creatureData.ForagingPriorityIncrease);

        [SerializeField]
        private BaseFood _targetFood;

        public override BehaviourState StateSuggestion => BehaviourState.FindingFood;

        public override bool IsEligibleForActivation => false;

        
        private CircleTrigger _findFoodTrigger;
        
        private void Start()
        {
            _findFoodTrigger = CircleTrigger.Create(_creatureData.FindFoodRange, "findFoodTrigger", transform);
            _findFoodTrigger.OnTriggerEnter += CheckForFoodEnterSight;
            _findFoodTrigger.OnTriggerExit += CheckForFoodLeaveSight;
        }

        private void OnDestroy()
        {
            _findFoodTrigger.OnTriggerEnter -= CheckForFoodEnterSight;
            _findFoodTrigger.OnTriggerExit -= CheckForFoodLeaveSight;
            Destroy(_findFoodTrigger.gameObject);
        }

        protected override void UpdateWhenActive()
        {
            bool iCanEat = _creature.Hunger >= _creatureData.StartHuntingThreshold;

            if (iCanEat == false || _targetFood == null || _targetFood.CanBeEaten() == false || _creature.Alive == false)
            {
                OnDeactivationRequest?.Invoke(this);
            }
            else
            {
                if (_creature.Movement.Moving)
                {
                    var targetPreyGrid = LevelManager.Instance.Grid.WorldToGridPosition(_targetFood.transform.position);
                    var walkTargetGrid = LevelManager.Instance.Grid.WorldToGridPosition(_creature.Movement.TargetPosition);

                    if (targetPreyGrid != walkTargetGrid)
                    {
                        MoveTowardsTarget();
                    }
                }
            }
        }

        protected override void UpdateWhenInactive()
        {
            if (_targetFood != null && _creature.Hunger >= _creatureData.StartForagingThreshold)
            {
                OnActivationRequest.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _findFoodTrigger.OnTriggerEnter -= CheckForFoodEnterSight;
                _findFoodTrigger.OnTriggerExit -= CheckForFoodLeaveSight;
                _creature.Movement.OnTargetReached += OnTargetReached;
                _creature.Movement.OnSetTargetFailed += MoveTowardsTarget;

                MoveTowardsTarget();
            }
            else
            {
                _findFoodTrigger.OnTriggerEnter += CheckForFoodEnterSight;
                _findFoodTrigger.OnTriggerExit += CheckForFoodLeaveSight;
                _creature.Movement.OnTargetReached -= OnTargetReached;
                _creature.Movement.OnSetTargetFailed -= MoveTowardsTarget;
                _targetFood = null;
                _creature.Movement.Stop();
            }
        }

        private void CheckForFoodEnterSight(Collider2D coll)
        {
            if (coll.isTrigger == false && _targetFood == null)
            {
                var possibleFood = coll.gameObject.GetComponent<BaseFood>();
                if (possibleFood == null)
                {
                    return;
                }
                if (possibleFood.CanBeEaten() && EatsFoodContainsFood(possibleFood))
                {
                    _targetFood = possibleFood;
                }
            }
        }

        private bool EatsFoodContainsFood(BaseFood food)
        {
            foreach (var f in _creatureData.EatsFood)
            {
                if (food.GetType() == f.GetType())
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckForFoodLeaveSight(Collider2D coll)
        {
            if (coll.isTrigger == false && _targetFood != null)
            {
                var possibleFoodTarget = coll.gameObject.GetComponent<BaseFood>();
                if (possibleFoodTarget == null)
                {
                    return;
                }
                if (possibleFoodTarget == _targetFood)
                {
                    _targetFood = null;
                }
            }
        }

        private void MoveTowardsTarget()
        {
            if (_targetFood != null)
            {
                _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
                _creature.Movement.SetTarget(_targetFood.transform.position);
            }
            else
            {
                OnDeactivationRequest?.Invoke(this);
            }
        }

        private void OnTargetReached()
        {
            OnDeactivationRequest?.Invoke(this);
        }
    }
}