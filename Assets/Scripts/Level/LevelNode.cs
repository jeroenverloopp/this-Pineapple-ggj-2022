using PathFinding.AStar;

namespace Level
{
    public class LevelNode : AStarNode
    {

        public int Nutrition;
        
        
        public LevelNode(int gridX, int gridY) : base(gridX, gridY)
        {
        }
    }
}