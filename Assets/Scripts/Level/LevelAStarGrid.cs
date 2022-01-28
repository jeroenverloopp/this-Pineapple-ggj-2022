using PathFinding.AStar;
using UnityEngine;

namespace Level
{
    public class LevelAStarGrid : AStarGrid{
        
        public LevelAStarGrid(Vector2 position, Vector2 gridSize, Vector2Int nodeCount) : base(position, gridSize, nodeCount)
        {
        }

        public override void CreateGrid()
        {
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.GetLength(0); y++)
                {
                    _grid[x, y] = new LevelNode(x, y);
                }
            }
        }
    }
}