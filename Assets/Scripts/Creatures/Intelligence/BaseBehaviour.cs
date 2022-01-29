using System;
using UnityEngine;

namespace Creatures.Behaviour
{
    public abstract class BaseBehaviour : MonoBehaviour
    {
        
        public abstract int Priority { get; }
        public abstract BehaviourState StateSuggestion { get; }
        public abstract bool IsEligibleForActivation { get; }
        
        public Action<BaseBehaviour> OnActivationRequest;
        public Action<BaseBehaviour> OnDeactivationRequest;
        
        public bool Active { get; private set; }

        protected BaseCreatureData _creatureData;
        protected BaseCreature _creature;
        
        public virtual void Set(BaseCreatureData creatureData, BaseCreature creature)
        {
            _creatureData = creatureData;
            _creature = creature;
        }

        public virtual void SetActive(bool active)
        {
            Active = active;
            if (Active)
            {
                //Debug.Log($"Setting {GetType().Name} active");
            }
        }

        void Update()
        {
            if (HasData())
            {
                if (Active)
                {
                    UpdateWhenActive();
                }
                else
                {
                    UpdateWhenInactive();
                }
            }
        }

        protected abstract void UpdateWhenActive();
        protected abstract void UpdateWhenInactive();

        protected bool HasData()
        {
            return _creatureData != null && _creature != null;
        }
        
    }
}