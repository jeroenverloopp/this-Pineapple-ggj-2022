using UnityEngine;

namespace Creatures.Behaviour
{
    public class DyingBehaviour : BaseBehaviour
    {
        [SerializeField]
        public override int Priority => _creature.Alive ? -1 : int.MaxValue;
        
        public override BehaviourState StateSuggestion => BehaviourState.Dieing;
        
        public override bool IsEligibleForActivation => false;
        
        [SerializeField]
        private float _durationTimer = 0;
        
        protected override void UpdateWhenActive()
        {
            _durationTimer = Mathf.Clamp(_durationTimer - Time.deltaTime, 0, _durationTimer);

            if (_durationTimer <= 0 || _creature.Nutrition <= 0)
            {
                Debug.Log("Died!!!");
                Destroy(gameObject);
            }
        }

        protected override void UpdateWhenInactive()
        {
            if (_creature.Alive == false)
            {
                OnActivationRequest?.Invoke(this);
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (Active)
            {
                _creature.Movement.Stop();
                _durationTimer = 8f;
            }
        }
    }
}