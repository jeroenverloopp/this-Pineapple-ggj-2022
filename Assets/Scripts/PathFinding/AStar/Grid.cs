using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.AStar
{

    public class Grid
    {


        public Vector2 NodeSize => _nodeSize;
        public Vector2Int NodeCount => _nodeCount;
        public int TotalNodes => _nodeCount.x * _nodeCount.y;

        public Vector2 Center => _position + _gridSize / 2;
        public Vector2 TopRight => _position + _gridSize;
        public Vector2 LeftBottom => _position;

        private Node[,] _grid;
        private Vector2 _position;
        private Vector2 _gridSize;
        private Vector2 _nodeSize;
        private Vector2Int _nodeCount;

        public Grid(Vector2 position, Vector2 gridSize, Vector2Int nodeCount)
        {
            _position = position;
            _gridSize = gridSize;
            _nodeCount = nodeCount;
            _grid = new Node[_nodeCount.x, _nodeCount.y];
            _nodeSize = new Vector2(_gridSize.x / _nodeCount.x, _gridSize.y / _nodeCount.y);

            CreateGrid();
        }

        public void ResetNodes()
        {
            foreach (var node in _grid)
            {
                node.Reset();
            }
        }

        public void SetWalkableByCollision(LayerMask collisionMask, float radius)
        {
            foreach (var node in _grid)
            {
                Vector2 center = GridToWorldPositionCentered(node.GridX, node.GridY);
                Vector2 size = _nodeSize / 2;

                Vector3 boxCenter = new Vector3(center.x, 0, center.y);

                if (Physics.CheckSphere(boxCenter, radius, collisionMask))
                {
                    node.SetWalkable(false);
                }
                else
                {
                    node.SetWalkable(true);
                }
            }
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && x < _nodeCount.x && y >= 0 && y < _nodeCount.y;
        }

        public List<Node> GetNodeNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    if (InBounds(node.GridX + x, node.GridY + y))
                    {
                        neighbours.Add(_grid[node.GridX + x, node.GridY + y]);
                    }
                }
            }

            return neighbours;
        }

        public Node NodeFromWorldPosition(Vector2 worldPosition)
        {
            return NodeFromWorldPosition(worldPosition.x, worldPosition.y);
        }

        public Node NodeFromWorldPosition(float worldX, float worldY)
        {
            Vector2Int gridPosition = WorldToGridPositionInBounds(worldX, worldY);
            return _grid[gridPosition.x, gridPosition.y];
        }

        public Vector2 GridToWorldPosition(Vector2Int gridPosition)
        {
            return GridToWorldPosition(gridPosition.x, gridPosition.y);
        }

        public Vector2 GridToWorldPosition(int gridX, int gridY)
        {
            return new Vector2(gridX * _nodeSize.x, gridY * _nodeSize.y) + _position;
        }

        public Vector2 GridToWorldPositionCentered(Vector2Int gridPosition)
        {
            return GridToWorldPositionCentered(gridPosition.x, gridPosition.y);
        }

        public Vector2 GridToWorldPositionCentered(int gridX, int gridY)
        {
            return GridToWorldPosition(gridX, gridY) + _nodeSize / 2;
        }

        public Vector2Int WorldToGridPosition(Vector2 worldPosition)
        {
            return WorldToGridPosition(worldPosition.x, worldPosition.y);
        }

        public Vector2Int WorldToGridPosition(float worldX, float worldY)
        {
            int gridX = Mathf.FloorToInt((worldX - _position.x) / _nodeSize.x);
            int gridY = Mathf.FloorToInt((worldY - _position.y) / _nodeSize.y);
            return new Vector2Int(gridX, gridY);
        }

        public Vector2Int WorldToGridPositionInBounds(Vector2 worldPosition)
        {
            return WorldToGridPositionInBounds(worldPosition.x, worldPosition.y);
        }

        public Vector2Int WorldToGridPositionInBounds(float worldX, float worldY)
        {
            Vector2Int gridPosition = WorldToGridPosition(worldX, worldY);
            return new Vector2Int(
                Mathf.Clamp(gridPosition.x, 0, _nodeCount.x - 1),
                Mathf.Clamp(gridPosition.y, 0, _nodeCount.y - 1)
            );
        }

        private void CreateGrid()
        {
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.GetLength(0); y++)
                {
                    _grid[x, y] = new Node(x, y);
                }
            }
        }
    }
}