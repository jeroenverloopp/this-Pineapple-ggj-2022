using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using PathFinding.AStar;
using UnityEngine;

namespace Creatures
{
    public class MovementComponent : MonoBehaviour
    {
        
        private float _speed;
        private List<Vector2> _waypoints;

        public Action OnTargetReached;
        public Action OnSetTargetFailed;
        
        public void Stop()
        {
            StopCoroutine(FollowPath());
            _waypoints = null;
        }
        
        public void SetTarget(Transform target)
        {
            SetTarget(target.position);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        
        public void SetTarget(Vector2 targetPosition)
        {
            Vector2 fromPosition = transform.position;
            PathRequestManager.RequestPath(LevelManager.Instance.Grid, fromPosition, targetPosition , SetPathToTarget);
        }

        private void SetPathToTarget(List<Vector2> waypoints, bool success)
        {
            if (success)
            {
                Stop();
                _waypoints = waypoints;
                StartCoroutine(FollowPath());
            }
            else
            {
                OnSetTargetFailed?.Invoke();
            }
        }

        private IEnumerator FollowPath()
        {
            Vector2 currentWaypoint = _waypoints[0];
            int targetIndex = 0;

            while (true)
            {
                Vector2 position = transform.position;
                if (position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= _waypoints.Count)
                    {
                        OnTargetReached?.Invoke();
                        yield break;
                    }
                    currentWaypoint = _waypoints[targetIndex];
                }
                
                Vector2 nextPos = Vector2.MoveTowards(position, currentWaypoint, _speed*Time.deltaTime);
                transform.position = nextPos;
                yield return null;
            }
        }
    }
}