using PathFinding.AStar;
using UnityEngine;

namespace Level
{
    public class LevelNode : AStarNode
    {

        public GroundType Type = GroundType.Dirt;

        
        public LevelNode(int gridX, int gridY) : base(gridX, gridY)
        {
        }

        public void SetType(AStarGrid grid, GroundType type)
        {
            Type = type;
            
        }
    }
}