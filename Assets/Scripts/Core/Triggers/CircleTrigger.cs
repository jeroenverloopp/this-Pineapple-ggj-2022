using System;
using UnityEngine;

namespace Core.Triggers
{
    public class CircleTrigger : MonoBehaviour
    {
        
        public static CircleTrigger Create(float radius, string name = "CircleTrigger", Transform parent = null)
        {
            GameObject go = new GameObject(name);
            CircleTrigger ct = go.AddComponent<CircleTrigger>();
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
            ct.Setup(radius);
            return ct;
        }

        public Action<Collider2D> OnTrigger;

        private CircleCollider2D _circleCollider;

        public void Setup(float radius)
        {
            _circleCollider = gameObject.AddComponent<CircleCollider2D>();
            _circleCollider.isTrigger = true;
            _circleCollider.radius = radius;
        }

        private void OnTriggerEnter2D(Collider2D colllider)
        {
            OnTrigger?.Invoke(colllider);
        }
    }
}