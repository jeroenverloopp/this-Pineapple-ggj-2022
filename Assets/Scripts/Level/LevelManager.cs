using System;
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
            Grid.SetWalkableByCollision(_collisionMask, Mathf.Max(Grid.NodeSize.x,Grid.NodeSize.y)/2);
            Grid.SetNeighbours();
            //Grid.MakeLevelTiles();
        }


        private void OnDrawGizmos()
        {
            if (Grid == null)
            {
                return;
            }
            for (int x = 0; x < Grid.NodeCount.x; x++)
            {
                for (int y = 0; y < Grid.NodeCount.y; y++)
                {
                    if (Grid[x, y] != null && Grid[x, y].Walkable == false)
                    {
                        Gizmos.color = Color.red;
                        Vector2 worldPos = Grid.GridToWorldPositionCentered(x, y);
                        Gizmos.DrawWireCube(new Vector3(worldPos.x, worldPos.y , 1), new Vector3(Grid.NodeSize.x,Grid.NodeSize.y,1));
                    }
                    
                }
            }
        }
    }
}