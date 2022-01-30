using System;
using UnityEngine;

namespace Icons
{
    public class IconComponent : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;
        private IconData _iconData;

        private Vector3 _startScale;
        private Color _startColor;
        
        public void Set(IconData iconData)
        {
            if (iconData == null)
            {
                Destroy(gameObject);
                return;
            }

            _iconData = iconData;

            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            _spriteRenderer.sortingLayerName = "Icons";
            _spriteRenderer.sprite = iconData.Icon;
            _spriteRenderer.color = iconData.Color;
            name = $"Icon[{iconData.Name}]";

            _startScale = gameObject.transform.localScale;
            _startColor = iconData.Color;
            transform.localScale = Vector3.zero;
        }



        private float _counter = 0;
        private float _scale = .5f;
        private float _alpha = 1;
        
        private void Update()
        {
            if (_iconData == null)
            {
                return;
            }
            
            
            transform.position += Vector3.up * Time.deltaTime*0.5f;

            _scale = Mathf.Min(_scale + Time.deltaTime*0.5f, 1);
            transform.localScale = Vector3.one*_scale;
            if (_scale >= 1)
            {
                _alpha = Mathf.Max(_alpha - Time.deltaTime, 0);
                _spriteRenderer.color = new Color(_startColor.r, _startColor.g, _startColor.b, _alpha);
                if (_alpha <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}