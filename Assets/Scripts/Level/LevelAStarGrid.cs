using System.Collections.Generic;
using PathFinding.AStar;
using UnityEngine;

namespace Level
{
    public class LevelAStarGrid : AStarGrid
    {

        private LevelTile[,] _tiles;
        private LevelTile _tilePrefab;
        
        public LevelAStarGrid(Vector2 position, Vector2 gridSize, Vector2Int nodeCount, LevelTile tilePrefab) : base(position, gridSize, nodeCount)
        {
            _tilePrefab = tilePrefab;
            _tiles = new LevelTile[nodeCount.x, nodeCount.y];
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

        public void MakeLevelTiles()
        {
            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(0); y++)
                {
                    var tile = GameObject.Instantiate(_tilePrefab);
                    tile.transform.position = GridToWorldPositionCentered(x, y);
                    var node = this[x, y];
                    tile.SetType(node.Walkable? GroundType.Grass : GroundType.Dirt);
                    _tiles[x, y] = tile;
                }
            }
            
            //MakeBiomes();
        }

        public void MakeBiomes()
        {
            int rndX = Random.Range(0, _nodeCount.x);
            int rndY = Random.Range(0, _nodeCount.y);

            List<LevelNode> openList = new List<LevelNode>();
            List<AStarNode> closedList = new List<AStarNode>();
            openList.Add((LevelNode)_grid[rndX,rndY]);

            int tries = 0;
            while (openList.Count > 0 && tries <1000)
            {
                tries++;
                LevelNode currentNode = openList[0];
                closedList.Add(currentNode);
                openList.RemoveAt(0);
                _tiles[currentNode.GridX,currentNode.GridY].SetType(GroundType.Dirt);
                foreach (var neighbour in currentNode.Neighbours)
                {
                    if (closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    float dist = Vector2Int.Distance(new Vector2Int(rndX, rndY), new Vector2Int(neighbour.GridX, neighbour.GridY));
                    
                    int rnd = Random.Range(0, 100);
                    if (rnd > 40+dist*4)
                    {
                        openList.Add((LevelNode)neighbour);
                    }
                }
            }
            
            Debug.Log($"closedList.Count {closedList.Count} : tries: {tries}");

        }
    }
}