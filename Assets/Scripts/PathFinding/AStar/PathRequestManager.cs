using System;
using System.Collections.Generic;
using Core.Singletons;
using UnityEngine;

namespace PathFinding.AStar
{
    public class PathRequestManager : MonoBehaviourLazySingleton<PathRequestManager>
    {
        private Queue<PathRequest> _pathRequestQueue = new();
        private PathRequest _currentPathRequest;

        private readonly AStar _aStar = new AStar();
        private bool _isProcessingPath;

        public static void RequestPath(Grid grid, Vector2 pathStart, Vector2 pathEnd, Action<List<Vector2>, bool> callback)
        {
            PathRequest pathRequest = new PathRequest(grid,pathStart, pathEnd, callback);
            Instance._pathRequestQueue.Enqueue(pathRequest);
            Instance.TryProcessNext();
        }


        void TryProcessNext()
        {
            if (_isProcessingPath == false && _pathRequestQueue.Count > 0)
            {
                _currentPathRequest = _pathRequestQueue.Dequeue();
                _isProcessingPath = true;
                StartCoroutine(_aStar.FindPath(_currentPathRequest));
            }
        }

        public void FinishedProcessingPath(List<Vector2> path, bool success)
        {
            _currentPathRequest.CallBack?.Invoke(path, success);
            _isProcessingPath = false;
            TryProcessNext();
        }
        
    }
}