using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Behaviour
{
    public class BehaviourManager : MonoBehaviour
    {

        private List<BaseBehaviour> _behaviourList;
        
        [SerializeField]//only for debug
        private BaseBehaviour _activeBehaviour;

        public void Init(List<BaseBehaviour> behaviourList , BaseCreatureData data, BaseCreature creature)
        {
            
            if (behaviourList == null)
            {
                Debug.LogError("BehaviourList == null");
                return;
            }

            _behaviourList = new List<BaseBehaviour>();
            foreach (var behaviour in behaviourList)
            {

                var component = (BaseBehaviour)gameObject.AddComponent(behaviour.GetType());
                component.Set(data, creature);
                component.OnActivationRequest += CheckForActivationOverride;
                component.OnDeactivationRequest += CheckForDeactivation;
                _behaviourList.Add(component);
            }
        }

        private void CheckForActivationOverride(BaseBehaviour overrideBehaviour)
        {
            if (_activeBehaviour != overrideBehaviour && overrideBehaviour.Priority > _activeBehaviour.Priority)
            {
                _activeBehaviour.SetActive(false);
                _activeBehaviour = overrideBehaviour;
                _activeBehaviour.SetActive(true);
            }
        }

        private void CheckForDeactivation(BaseBehaviour overrideBehaviour)
        {
            if (_activeBehaviour == overrideBehaviour)
            {
                _activeBehaviour.SetActive(false);
                Debug.Log($"Setting {_activeBehaviour.GetType().Name} inactive");
                _activeBehaviour = null;
            }
        }


        private void Update()
        {
            if (_activeBehaviour == null)
            {
                _activeBehaviour = FindBehaviourEligibleForActivation();
                if (_activeBehaviour != null)
                {
                    _activeBehaviour.SetActive(true);
                    Debug.Log($"Setting {_activeBehaviour.GetType().Name} active");
                }
            }
        }



        private BaseBehaviour FindBehaviourEligibleForActivation()
        {
            List<BaseBehaviour> foundBehaviours = new List<BaseBehaviour>();
            int currentPriority = -1;

            foreach (var behaviour in _behaviourList)
            {
                if (behaviour.Priority > currentPriority)
                {
                    foundBehaviours.Clear();
                    foundBehaviours.Add(behaviour);
                    currentPriority = behaviour.Priority;
                }
                else if (behaviour.Priority == currentPriority)
                {
                    foundBehaviours.Add(behaviour);
                }
            }

            if (foundBehaviours.Count > 0)
            {
                var chosen = foundBehaviours[Random.Range(0, foundBehaviours.Count)];
                return chosen;
            }

            return null;
        }
    }
}