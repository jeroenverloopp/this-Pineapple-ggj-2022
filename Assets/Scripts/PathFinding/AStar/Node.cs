using Util.Collections;

namespace PathFinding.AStar
{
    public class Node : IHeapItem<Node>
    {

        public int HeapIndex { get; set; }
        public bool Walkable { get; private set; } = true;

        public int HCost, GCost;
        public int FCost => HCost + GCost;
        
        public Node Parent { get; private set; }

        public readonly int GridX, GridY;

        public Node(int gridX, int gridY)
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
        

        public void SetWalkable(bool walkable)
        {
            Walkable = walkable;
        }

        public void SetParent(Node parent)
        {
            Parent = parent;
        }

        public int CompareTo(Node other)
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