using System;
using System.Collections.Generic;
using Core.Singletons;
using UnityEngine;
using Util;

namespace PathFinding.AStar
{
    public static class PathRequestManager
    {
        private static readonly Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
        private static PathRequest _currentPathRequest;

        private static readonly AStar _aStar = new AStar();
        private static bool _isProcessingPath;

        public static void RequestPath(AStarGrid aStarGrid, Vector2 pathStart, Vector2 pathEnd, Action<List<Vector2>, bool> callback)
        {
            PathRequest pathRequest = new PathRequest(aStarGrid,pathStart, pathEnd, callback);
            _pathRequestQueue.Enqueue(pathRequest);
            TryProcessNext();
        }


        private static void TryProcessNext()
        {
            if (_isProcessingPath == false && _pathRequestQueue.Count > 0)
            {
                _currentPathRequest = _pathRequestQueue.Dequeue();
                _isProcessingPath = true;
                CoroutineHelper.Instance.StartCoroutine(_aStar.FindPath(_currentPathRequest));
            }
        }

        public static void FinishedProcessingPath(List<Vector2> path, bool success)
        {
            _currentPathRequest.CallBack?.Invoke(path, success);
            _isProcessingPath = false;
            TryProcessNext();
        }
        
    }
}