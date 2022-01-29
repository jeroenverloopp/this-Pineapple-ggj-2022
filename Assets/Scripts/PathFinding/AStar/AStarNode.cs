using System.Collections.Generic;
using Util.Collections;

namespace PathFinding.AStar
{
    public class AStarNode : IHeapItem<AStarNode>
    {

        public int HeapIndex { get; set; }
        public bool Walkable { get; private set; } = true;
        public int HCost, GCost;
        public int FCost => HCost + GCost;
        public AStarNode Parent { get; private set; }

        public List<AStarNode> Neighbours { get; private set; } = new List<AStarNode>();
        public List<AStarNode> WalkableNeighbours { get; private set; } = new List<AStarNode>();

        public readonly int GridX, GridY;

        public AStarNode(int gridX, int gridY)
        {
            GridX = gridX;
            GridY = gridY;
            SetWalkable(true);
        }

        public void Reset()
        {
            HCost = 0;
            GCost = 0;
            HeapIndex = 0;
            Parent = null;
        }


        public void AddNeighbour(AStarNode node)
        {
            Neighbours.Add(node);
            if (node.Walkable)
            {
                WalkableNeighbours.Add(node);
            }
        }
        
        public void SetWalkable(bool walkable)
        {
            Walkable = walkable;
        }

        public void SetParent(AStarNode parent)
        {
            Parent = parent;
        }

        public int CompareTo(AStarNode other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }

            return -compare;
        }
        
    }
}