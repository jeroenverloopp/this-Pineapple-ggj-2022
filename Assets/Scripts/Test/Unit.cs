using System;
using System.Collections;
using System.Collections.Generic;
using PathFinding.AStar;
using UnityEngine;
using Util;
using Grid = PathFinding.AStar.Grid;
using Random = UnityEngine.Random;

namespace Test
{
    public class Unit : MonoBehaviour
    {

        private Transform _target;
        private float _speed;
        private List<Vector2> _waypoints;
        private void Awake()
        {
            _speed = Random.Range(3.0f, 5.0f);
        }


        public void SetTarget(Grid grid, Transform target)
        {
            _target = target;
            Vector2 from = Dimensions.TransformPosition2D(transform);
            Vector2 to = Dimensions.TransformPosition2D(target);
            PathRequestManager.RequestPath(grid, from, to , SetPathToTarget);
        }

        private void SetPathToTarget(List<Vector2> waypoints, bool success)
        {
            if (success)
            {
                _waypoints = waypoints;
                StopCoroutine(FollowPath());
                StartCoroutine(FollowPath());
            }
        }

        private IEnumerator FollowPath()
        {
            Vector2 currentWaypoint = _waypoints[0];
            int targetIndex = 0;

            while (true)
            {
                Vector2 position = Dimensions.TransformPosition2D(transform);
                if (position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= _waypoints.Count)
                    {
                        yield break;
                    }
                    currentWaypoint = _waypoints[targetIndex];
                }

                //Debug.Log($"{position} -> {currentWaypoint}");
                Vector2 nextPos = Vector2.MoveTowards(position, currentWaypoint, _speed*Time.deltaTime);
                transform.position = Dimensions.TransformPosition3D(nextPos , 1);
                yield return null;
            }
        }
        
    }
}