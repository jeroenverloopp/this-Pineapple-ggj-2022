using Core.Singletons;
using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviourSingleton<LevelManager>
    {
        
        public LevelAStarGrid Grid { get; private set; }

        [SerializeField] private LayerMask _collisionMask;
        [SerializeField] private LevelTile _tilePrefab;


        [SerializeField] private Vector2 _gridPosition;
        [SerializeField] private Vector2 _gridSize;
        [SerializeField] private Vector2Int _nodeCount;
        
        protected override void Awake()
        {
            base.Awake();
            Grid = new LevelAStarGrid(_gridPosition, _gridSize, _nodeCount, _tilePrefab);
            Grid.SetWalkableByCollision(_collisionMask, 2);
            Grid.SetNeighbours();
            //Grid.MakeLevelTiles();
        }
    }
}