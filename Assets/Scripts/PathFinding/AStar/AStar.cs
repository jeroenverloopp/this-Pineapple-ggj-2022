using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Util.Collections;
using Debug = UnityEngine.Debug;

namespace PathFinding.AStar
{
    public class AStar
    {

        public IEnumerator FindPath(PathRequest pathRequest)
        {
            return FindPath(pathRequest.PathStart, pathRequest.PathEnd, pathRequest.Grid);
        }
        
        public IEnumerator FindPath(Vector2 startPosition, Vector2 endPosition, Grid grid)
        {
            grid.ResetNodes();
            Node startNode = grid.NodeFromWorldPosition(startPosition);
            Node targetNode = grid.NodeFromWorldPosition(endPosition);

            Heap<Node> openSet = new Heap<Node>(grid.TotalNodes);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            List<Vector2> pathToTarget = null;
            
            if(startNode.Walkable && targetNode.Walkable){
                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirstItem();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathToTarget = RetracePath(grid, targetNode);
                        break;
                    }


                    foreach (Node neighbour in grid.GetNodeNeighbours(currentNode))
                    {
                        if (neighbour.Walkable == false || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCost = currentNode.GCost + GetDistance(currentNode, neighbour);
                        if (newMovementCost < neighbour.GCost || openSet.Contains(neighbour) == false)
                        {
                            neighbour.GCost = newMovementCost;
                            neighbour.HCost = GetDistance(neighbour, targetNode);
                            neighbour.SetParent(currentNode);

                            if (openSet.Contains(neighbour) == false)
                            {
                                openSet.Add(neighbour);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }
                    }
                }
            }
            yield return null;
            
            if (pathToTarget != null)
            {
                PathRequestManager.Instance.FinishedProcessingPath(pathToTarget, true);
            }
            else
            {
                PathRequestManager.Instance.FinishedProcessingPath(null,false);
            }
        }

        List<Vector2> RetracePath(Grid grid, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            path.Add(currentNode);

            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
                path.Add(currentNode);
            }

            List<Vector2> wayPoints = SimplifyPath(grid, path);
            wayPoints.Reverse();
            return wayPoints;
        }

        List<Vector2> SimplifyPath(Grid grid, List<Node> path)
        {
            List<Vector2> wayPoints = new List<Vector2>();
            Vector2 directionOld = Vector2.zero;
            for (int i = 1; i < path.Count; i++)
            {
                int xDirection = path[i - 1].GridX - path[i].GridX;
                int yDirection = path[i - 1].GridY - path[i].GridY;
                Vector2 direction = new Vector2(xDirection, yDirection);
                if (direction != directionOld)
                {
                    wayPoints.Add(grid.GridToWorldPositionCentered(path[i].GridX, path[i].GridY));
                    directionOld = direction;
                }
            }

            return wayPoints;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);
            int diff = Mathf.Abs(distX - distY);
            int max = Mathf.Max(distX, distY);
            return 10 * diff + (max - diff) * 14;
        }

    }
}