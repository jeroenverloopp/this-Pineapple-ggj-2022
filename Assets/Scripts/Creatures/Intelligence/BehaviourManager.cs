using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Behaviour
{
    public class BehaviourManager : MonoBehaviour
    {

        private List<BaseBehaviour> _behaviourList;
        
        [SerializeField]//only for debug
        private BaseBehaviour _activeBehaviour;

        private BaseCreature _creature;

        public void Init(List<BaseBehaviour> behaviourList , BaseCreatureData data, BaseCreature creature)
        {

            _creature = creature;
            
            if (behaviourList == null)
            {
                Debug.LogError("BehaviourList == null");
                return;
            }

            if (_behaviourList != null)
            {
                foreach (var b in _behaviourList)
                {
                    b.OnActivationRequest -= CheckForActivationOverride;
                    b.OnDeactivationRequest -= CheckForDeactivation;
                    Destroy(b);
                }
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
            if (_activeBehaviour == null)
            {
                _activeBehaviour = overrideBehaviour;
                _activeBehaviour.SetActive(true);
            }
            else if (_activeBehaviour.GetType() != overrideBehaviour.GetType() && overrideBehaviour.Priority > _activeBehaviour.Priority)
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
                }
            }

            if (_activeBehaviour != null)
            {
                if (_creature.State != _activeBehaviour.StateSuggestion)
                {
                    _creature.SetState(_activeBehaviour.StateSuggestion);
                }
            }
        }



        private BaseBehaviour FindBehaviourEligibleForActivation()
        {
            List<BaseBehaviour> foundBehaviours = _behaviourList.Where(x => x.IsEligibleForActivation == true).ToList();
            if (foundBehaviours.Count > 0)
            {
                foundBehaviours = foundBehaviours.Where(x => x.Priority == foundBehaviours[0].Priority).ToList();
            }

            if (foundBehaviours.Count > 0)
            {
                return foundBehaviours[Random.Range(0, foundBehaviours.Count)];
            }

            return null;
        }
    }
}