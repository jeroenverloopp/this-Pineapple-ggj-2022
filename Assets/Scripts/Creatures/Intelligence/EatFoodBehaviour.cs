using Audio;
using Core.Triggers;
using Food;
using Icons;
using UnityEngine;

namespace Creatures.Behaviour
{
    public class EatFoodBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => 100;
        
        public override BehaviourState StateSuggestion => BehaviourState.Eating;
        
        public override bool IsEligibleForActivation => false;

        private BaseFood _eatableFood;
        private CircleTrigger _eatFoodTrigger;
        private float _eatingTimer;
        
        private void Start()
        {
            _eatFoodTrigger = CircleTrigger.Create(_creatureData.EatFoodRange, "eatFoodTrigger", transform);
            _eatFoodTrigger.OnTriggerEnter += CheckForEatableFoodFound;
            _eatFoodTrigger.OnTriggerExit += CheckForEatableFoodLost;
        }
        
        private void OnDestroy()
        {
            _eatFoodTrigger.OnTriggerEnter -= CheckForEatableFoodFound;
            _eatFoodTrigger.OnTriggerExit -= CheckForEatableFoodLost;
        }
        
        protected override void UpdateWhenActive()
        {
            if (_eatableFood == null)
            {
                OnDeactivationRequest?.Invoke(this);
                return;
            }

            if (_creature.Alive == false)
            {
                if (_eatableFood.Eater == _creature)
                {
                    _eatableFood.StopEating(_creature);
                }
                OnDeactivationRequest?.Invoke(this);
                return;
            }

            if (_eatableFood.CanBeEaten() == false)
            {
                _eatableFood.StopEating(_creature);
                _eatableFood = null;
                return;
            }
            _eatingTimer = Mathf.Max(_eatingTimer - Time.deltaTime, 0);
            if (_eatingTimer <= 0)
            {
                _creature.PlayEatSound();
                IconManager.Instance.Create("Food", _eatableFood.transform.position + Vector3.up*.2f);
                _creature.Hunger = Mathf.Max(0, _creature.Hunger - _eatableFood.Eat());
                _eatableFood = null;
            }
        }

        protected override void UpdateWhenInactive()
        {
            if (_eatableFood != null && _creature.Hunger >= _creatureData.StartForagingThreshold)
            {
                OnActivationRequest?.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (Active)
            {
                _eatableFood.StartEating(_creature);
                _eatingTimer = _eatableFood.TimeUntilEaten;
            }
            else
            {
                _eatableFood = null;
            }
        }

        private void CheckForEatableFoodFound(Collider2D coll)
        {
            if (coll.isTrigger == false && _eatableFood == null)
            {
                var possibleFood = coll.gameObject.GetComponent<BaseFood>();
                if (possibleFood == null)
                {
                    return;
                }

                if (possibleFood.InUse == false && possibleFood.CanBeEaten() && EatsFoodContainsFood(possibleFood))
                {
                    _eatableFood = possibleFood;
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
        
        private void CheckForEatableFoodLost(Collider2D coll)
        {
            if (coll.isTrigger == false && _eatableFood != null)
            {
                var possibleFood = coll.gameObject.GetComponent<BaseFood>();
                if (possibleFood == null)
                {
                    return;
                }
                if (possibleFood == _eatableFood)
                {
                    _eatableFood = null;
                }
            }
        }
    }
}