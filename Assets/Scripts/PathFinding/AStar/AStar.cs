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
            return FindPath(pathRequest.PathStart, pathRequest.PathEnd, pathRequest.AStarGrid);
        }
        
        public IEnumerator FindPath(Vector2 startPosition, Vector2 endPosition, AStarGrid aStarGrid)
        {
            aStarGrid.ResetNodes();
            AStarNode startAStarNode = aStarGrid.NodeFromWorldPosition(startPosition);
            AStarNode targetAStarNode = aStarGrid.NodeFromWorldPosition(endPosition);

            Heap<AStarNode> openSet = new Heap<AStarNode>(aStarGrid.TotalNodes);
            HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
            openSet.Add(startAStarNode);

            List<Vector2> pathToTarget = null;
            
            if(startAStarNode.Walkable && targetAStarNode.Walkable){
                while (openSet.Count > 0)
                {
                    AStarNode currentAStarNode = openSet.RemoveFirstItem();
                    closedSet.Add(currentAStarNode);

                    if (currentAStarNode == targetAStarNode)
                    {
                        pathToTarget = RetracePath(aStarGrid, targetAStarNode);
                        break;
                    }


                    foreach (AStarNode neighbour in currentAStarNode.Neighbours)
                    {
                        if (neighbour.Walkable == false || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCost = currentAStarNode.GCost + GetDistance(currentAStarNode, neighbour);
                        if (newMovementCost < neighbour.GCost || openSet.Contains(neighbour) == false)
                        {
                            neighbour.GCost = newMovementCost;
                            neighbour.HCost = GetDistance(neighbour, targetAStarNode);
                            neighbour.SetParent(currentAStarNode);

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
            yield return new WaitForSeconds(.025f);
            
            if (pathToTarget != null)
            {
                PathRequestManager.FinishedProcessingPath(pathToTarget, true);
            }
            else
            {
                PathRequestManager.FinishedProcessingPath(null,false);
            }
        }

        List<Vector2> RetracePath(AStarGrid grid, AStarNode endAStarNode)
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode currentAStarNode = endAStarNode;
            path.Add(currentAStarNode);

            while (currentAStarNode.Parent != null)
            {
                currentAStarNode = currentAStarNode.Parent;
                path.Add(currentAStarNode);
            }

            List<Vector2> wayPoints = SimplifyPath(grid, path);
            wayPoints.Reverse();
            return wayPoints;
        }

        List<Vector2> SimplifyPath(AStarGrid grid, List<AStarNode> path)
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

        private int GetDistance(AStarNode aStarNodeA, AStarNode aStarNodeB)
        {
            int distX = Mathf.Abs(aStarNodeA.GridX - aStarNodeB.GridX);
            int distY = Mathf.Abs(aStarNodeA.GridY - aStarNodeB.GridY);
            int diff = Mathf.Abs(distX - distY);
            int max = Mathf.Max(distX, distY);
            return 10 * diff + (max - diff) * 14;
        }

    }
}