using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.AStar
{
    public struct PathRequest
    {
        public readonly Vector2 PathStart;
        public readonly Vector2 PathEnd;
        public readonly Grid Grid;
        public readonly Action<List<Vector2>, bool> CallBack;

        public PathRequest(Grid grid, Vector2 pathStart, Vector2 pathEnd, Action<List<Vector2>, bool> callBack)
        {
            Grid = grid;
            PathStart = pathStart;
            PathEnd = pathEnd;
            CallBack = callBack;
        }
        
    }
}