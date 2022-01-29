using UnityEngine;

namespace Level
{
    public class LevelTile : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer _spriteRenderer;


        public void SetType(GroundType groundType)
        {
            _spriteRenderer.color = groundType switch
            {
                GroundType.Dirt => new Color(.5f, .3f, 0.2f),
                GroundType.Grass => new Color(0, 1, 0),
                _ => Color.black
            };
        }
        
    }
}