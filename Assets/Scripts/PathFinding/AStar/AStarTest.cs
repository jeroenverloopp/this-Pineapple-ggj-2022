using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.AStar
{
    public class AStarTest : MonoBehaviour
    {

        private Grid _grid;
        
        private void Awake()
        {
            _grid = new Grid(Vector2.zero, new Vector2(100, 100), new Vector2Int(50, 50));
        }

        private void CreateSearchRequest()
        {
            if (_grid == null)
            {
                Debug.LogError("Define Grid before Creating a search request");
            }
            Vector2 startPosition = new Vector2(UnityEngine.Random.Range(0, 80), UnityEngine.Random.Range(0, 80));
            Vector2 targetPosition = new Vector2(UnityEngine.Random.Range(0, 80), UnityEngine.Random.Range(0, 80));
            //startPosition = new Vector2(46, 32);
            //targetPosition = new Vector2(13, 19);
            Debug.Log($"CreateSearchRequest: {startPosition} - {targetPosition}");
            PathRequestManager.RequestPath(_grid, startPosition, targetPosition, OnPathFound);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                for (int i = 0; i < 1000; i++)
                {
                    CreateSearchRequest();
                }
            }
        }
        
        
        private void OnPathFound (List<Node> path, bool foundPath)
        {
            if (foundPath)
            {
                Debug.Log($"Path found: Steps: {path.Count}");
            }
            else
            {
                Debug.Log($"Path not found");
            }
        }
    }
}