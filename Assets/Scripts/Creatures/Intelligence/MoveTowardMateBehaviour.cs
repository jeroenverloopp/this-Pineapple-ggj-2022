using System;
using Core.Triggers;
using Level;
using UnityEngine;

namespace Creatures.Behaviour
{
    public class MoveTowardMateBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => Mathf.RoundToInt(_creature.Reproduce / 25);

        [SerializeField]
        private BaseCreature _targetMate;

        public override BehaviourState StateSuggestion => BehaviourState.FindingMate;

        public override bool IsEligibleForActivation => false;

        
        private CircleTrigger _findMateRangeTrigger;
        
        private void Start()
        {
            _findMateRangeTrigger = CircleTrigger.Create(_creatureData.FindMateRange, "FindMateRange", transform);
            _findMateRangeTrigger.OnTriggerEnter += CheckForMateEnterSight;
            _findMateRangeTrigger.OnTriggerExit += CheckForMateLeaveSight;
        }

        private void OnDestroy()
        {
            _findMateRangeTrigger.OnTriggerEnter -= CheckForMateEnterSight;
            _findMateRangeTrigger.OnTriggerExit -= CheckForMateLeaveSight;
            Destroy(_findMateRangeTrigger.gameObject);
        }

        protected override void UpdateWhenActive()
        {
            bool iCanBreed = _creature.Reproduce >= _creatureData.CanReproduceThreshold;
            bool targetMateCanBreed = _targetMate != null && _targetMate.Reproduce >= _targetMate.creatureData.CanReproduceThreshold;
            
            
            if (iCanBreed == false || targetMateCanBreed == false || _targetMate.State == BehaviourState.Breeding || _creature.Alive == false)
            {
                OnDeactivationRequest?.Invoke(this);
            }
            else
            {
                if (_creature.Movement.Moving)
                {
                    var targetMateGrid = LevelManager.Instance.Grid.WorldToGridPosition(_targetMate.transform.position);
                    var walkTargetGrid = LevelManager.Instance.Grid.WorldToGridPosition(_creature.Movement.TargetPosition);

                    if (targetMateGrid != walkTargetGrid)
                    {
                        MoveTowardsTarget();
                    }
                }
            }
        }

        protected override void UpdateWhenInactive()
        {
            if (_targetMate != null && _creature.Reproduce >= _creatureData.CanReproduceThreshold)
            {
                OnActivationRequest?.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                _findMateRangeTrigger.OnTriggerEnter -= CheckForMateEnterSight;
                _findMateRangeTrigger.OnTriggerExit -= CheckForMateLeaveSight;
                _creature.Movement.OnTargetReached += OnTargetReached;
                _creature.Movement.OnSetTargetFailed += MoveTowardsTarget;

                MoveTowardsTarget();
            }
            else
            {
                _findMateRangeTrigger.OnTriggerEnter += CheckForMateEnterSight;
                _findMateRangeTrigger.OnTriggerExit += CheckForMateLeaveSight;
                _creature.Movement.OnTargetReached -= OnTargetReached;
                _creature.Movement.OnSetTargetFailed -= MoveTowardsTarget;
                _targetMate = null;
                _creature.Movement.Stop();
            }
        }

        private void CheckForMateEnterSight(Collider2D collider)
        {
            if (collider.isTrigger == false && _targetMate == null)
            {
                var creature = collider.gameObject.GetComponent<BaseCreature>();
                if (creature == null)
                {
                    return;
                }

                if (creature.GetType() == _creature.GetType() && _creature.Reproduce >= _creatureData.CanReproduceThreshold &&_creature.State != BehaviourState.Breeding)
                {
                    _targetMate = creature;
                }
            }
        }

        private void CheckForMateLeaveSight(Collider2D collider)
        {
            if (collider.isTrigger == false && _targetMate != null)
            {
                var creature = collider.gameObject.GetComponent<BaseCreature>();
                if (creature == null)
                {
                    return;
                }
                if (creature.GetType() == _targetMate.GetType())
                {
                    _targetMate = null;
                }
            }
        }

        private void MoveTowardsTarget()
        {
            if (_targetMate != null)
            {
                _creature.Movement.SetSpeed(_creatureData.MoveSpeed);
                _creature.Movement.SetTarget(_targetMate.transform.position);
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