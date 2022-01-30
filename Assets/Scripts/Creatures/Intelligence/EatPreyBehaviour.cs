using Audio;
using Core.Triggers;
using Icons;
using UnityEngine;

namespace Creatures.Behaviour
{
    public class EatPreyBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => 100;
        
        public override BehaviourState StateSuggestion => BehaviourState.Eating;
        
        public override bool IsEligibleForActivation => false;

        private BaseCreature _eatablePrey;
        private CircleTrigger _eatPreyTrigger;
        private float _eatingTimer;
        
        private void Start()
        {
            _eatPreyTrigger = CircleTrigger.Create(_creatureData.EatRange, "findPreyTrigger", transform);
            _eatPreyTrigger.OnTriggerEnter += CheckForEatablePreyFound;
            _eatPreyTrigger.OnTriggerExit += CheckForEatablePreyLost;
        }
        
        private void OnDestroy()
        {
            _eatPreyTrigger.OnTriggerEnter -= CheckForEatablePreyFound;
            _eatPreyTrigger.OnTriggerExit -= CheckForEatablePreyLost;
        }
        
        protected override void UpdateWhenActive()
        {
            if (_eatablePrey == null)
            {
                OnDeactivationRequest?.Invoke(this);
                return;
            }

            _eatingTimer = Mathf.Max(_eatingTimer - Time.deltaTime, 0);
            if (_eatingTimer <= 0)
            {
                Debug.Log("Prey eaten");
                AudioManager.Instance.Play("TeethEat");
                IconManager.Instance.Create("Food", _eatablePrey.transform.position + Vector3.up*.2f);
                _creature.Eat(_eatablePrey);
                _eatablePrey = null;
            }
        }

        protected override void UpdateWhenInactive()
        {
            if (_eatablePrey != null && _creature.Hunger >= _creatureData.StartHuntingThreshold)
            {
                OnActivationRequest?.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (Active)
            {
                _creature.Kill(_eatablePrey);
                _eatingTimer = _eatablePrey.creatureData.TimeToGetEaten;
            }
            else
            {
                _eatablePrey = null;
            }
        }

        private void CheckForEatablePreyFound(Collider2D coll)
        {
            if (coll.isTrigger == false && _eatablePrey == null)
            {
                var possiblePrey = coll.gameObject.GetComponent<BaseCreature>();
                if (possiblePrey == null)
                {
                    return;
                }

                if (_creatureData.EatsCreatures.Contains(possiblePrey.creatureData))
                {
                    _eatablePrey = possiblePrey;
                }
            }
        }
        
        private void CheckForEatablePreyLost(Collider2D coll)
        {
            if (coll.isTrigger == false && _eatablePrey != null)
            {
                var possiblePrey = coll.gameObject.GetComponent<BaseCreature>();
                if (possiblePrey == null)
                {
                    return;
                }
                if (possiblePrey == _eatablePrey)
                {
                    _eatablePrey = null;
                }
            }
        }
    }
}