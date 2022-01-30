using UnityEngine;

namespace Food
{
    public class BerryPlant : BaseFood
    {

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _hasBerry;
        [SerializeField] private Sprite _noBerry;
        [SerializeField] private float _resetDuration;
        
        private bool _canBeEaten;
        private float _cooldownCounter = 0;

        private void Awake()
        {
            SetCanBeEaten(true);
        }

        private void Update()
        {
            if (_canBeEaten == false)
            {
                _cooldownCounter = Mathf.Max(_cooldownCounter - Time.deltaTime, 0);
                if (_cooldownCounter <= 0)
                {
                    SetCanBeEaten(true);
                }
            }
        }

        public override bool CanBeEaten()
        {
            return _canBeEaten;
        }

        public override int Eat()
        {
            if (_canBeEaten)
            {
                SetCanBeEaten(false);
                return _nutrition;
            }

            return 0;
        }


        private void SetCanBeEaten(bool canBeEaten)
        {
            _canBeEaten = canBeEaten;
            if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = _canBeEaten ? _hasBerry : _noBerry;
            }

            if (_canBeEaten == false)
            {
                _cooldownCounter = _resetDuration;
            }
            else
            {
                InUse = false;
            }
        }
    }
}