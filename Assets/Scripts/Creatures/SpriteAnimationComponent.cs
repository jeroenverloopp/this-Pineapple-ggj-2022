using System.Collections.Generic;
using UnityEngine;

namespace Creatures
{
    public class SpriteAnimationComponent : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _spriteRenderList;

        private SpriteAnimation _animation;
        private int _index = 0;
        private float _counter = 0;
        
        
        public void SetAnimation(SpriteAnimation spriteAnimation)
        {
            _animation = spriteAnimation;
            _counter = 0;
            _index = 0;
            if (_animation != null && _animation.Sprites.Count > 0)
            {
                SetSprite(_animation.Sprites[0]);
            }
        }


        void Update()
        {
            if (_animation == null || _animation.Sprites == null || _animation.Sprites.Count == 0)
            {
                return;
            }

            _counter += Time.deltaTime * _animation.Speed;
            while (_counter > 1)
            {
                _counter--;
                _index++;
                if (_index >= _animation.Sprites.Count)
                {
                    _index = 0;
                }
                SetSprite(_animation.Sprites[_index]);
            }
        }

        private void SetSprite(Sprite sprite)
        {
            if (_spriteRenderList == null)
            {
                return;
            }
            
            foreach (var spriteRenderer in _spriteRenderList)
            {
                spriteRenderer.sprite = sprite;
            }
        }

    }
}