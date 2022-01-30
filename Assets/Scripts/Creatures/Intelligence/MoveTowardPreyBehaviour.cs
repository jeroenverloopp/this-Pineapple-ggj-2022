using System;
using Core.Triggers;
using Level;
using UnityEngine;

namespace Creatures.Behaviour
{
    public class MoveTowardPreyBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => Mathf.RoundToInt(_creature.Hunger-_creatureData.StartHuntingThreshold * _creatureData.HuntingPriorityIncrease);

        [SerializeField]
        private BaseCreature _targetPrey;

        public override BehaviourState StateSuggestion => BehaviourState.FindingPrey;

        public override bool IsEligibleForActivation => false;

        
        private CircleTrigger _findPreyTrigger;
        
        private void Start()
        {
            _findPreyTrigger = CircleTrigger.Create(_creatureData.FindPreyRange, "findPreyTrigger", transform);
            _findPreyTrigger.OnTriggerEnter += CheckForPreyEnterSight;
            _findPreyTrigger.OnTriggerExit += CheckForPreyLeaveSight;
        }

        private void OnDestroy()
        {
            _findPreyTrigger.OnTriggerEnter -= CheckForPreyEnterSight;
            _findPreyTrigger.OnTriggerExit -= CheckForPreyLeaveSight;
            _creature.Movement.OnTargetReached -= OnTargetReached;
            _creature.Movement.OnSetTargetFailed -= MoveTowardsTarget;
            Destroy(_findPreyTrigger.gameObject);
        }

        protected override void UpdateWhenActive()
        {
            bool iCanEat = _creature.Hunger >= _creatureData.StartHuntingThreshold;

            if (iCanEat == false || _targetPrey == null || _targetPrey.Alive == false || _creature.Alive == false)
            {
                _targetPrey = null;
                OnDeactivationRequest.Invoke(this);
            }
            else if(_targetPrey != null)
            {
                if (_creature.Movement.WaitingForPath == false)
                {
                    var targetPreyGrid = LevelManager.Instance.Grid.WorldToGridPosition(_targetPrey.transform.position);
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
            Debug.Log($"Trying to Activate Move toward prey: {_targetPrey != null && _creature.Hunger >= _creatureData.StartHuntingThreshold}");
            
            if (_targetPrey != null && _creature.Hunger >= _creatureData.StartHuntingThreshold)
            {
                OnActivationRequest.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _creature.Movement.OnTargetReached += OnTargetReached;
                _creature.Movement.OnSetTargetFailed += MoveTowardsTarget;
                MoveTowardsTarget();
            }
            else
            {
                _creature.Movement.OnTargetReached -= OnTargetReached;
                _creature.Movement.OnSetTargetFailed -= MoveTowardsTarget;
                _targetPrey = null;
                _creature.Movement.Stop();
            }
        }

        private void CheckForPreyEnterSight(Collider2D coll)
        {
            if (coll.isTrigger == false && _targetPrey == null)
            {
                var possiblePrey = coll.gameObject.GetComponent<BaseCreature>();
                if (possiblePrey == null)
                {
                    return;
                }

                if (possiblePrey.Alive && _creatureData.EatsCreatures.Contains(possiblePrey.creatureData))
                {
                    _targetPrey = possiblePrey;
                }
            }
        }

        private void CheckForPreyLeaveSight(Collider2D coll)
        {
            if (coll.isTrigger == false && _targetPrey != null)
            {
                var possibleTargetPrey = coll.gameObject.GetComponent<BaseCreature>();
                if (possibleTargetPrey == null)
                {
                    return;
                }
                if (possibleTargetPrey == _targetPrey)
                {
                    _targetPrey = null;
                }
            }
        }

        private void MoveTowardsTarget()
        {
            if (_targetPrey != null)
            {
                _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
                _creature.Movement.SetTarget(_targetPrey.transform.position);
            }
            else
            {
                OnDeactivationRequest.Invoke(this);
            }
        }

        private void OnTargetReached()
        {
            var targetPreyGrid = LevelManager.Instance.Grid.WorldToGridPosition(_targetPrey.transform.position);
            var transformGrid = LevelManager.Instance.Grid.WorldToGridPosition(transform.position);

            if (transformGrid == targetPreyGrid)
            {
                OnDeactivationRequest.Invoke(this);
            }
            else if(_creature.Movement.WaitingForPath == false)
            {
                MoveTowardsTarget();
            }
        }
    }
}