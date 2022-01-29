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
        
        public bool Moving { get; private set; }
        public Vector2 TargetPosition { get; private set; }
        
        public void Stop()
        {
            StopCoroutine(nameof(FollowPath));
            _waypoints = null;
            Moving = false;
        }
        
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        
        public void SetTarget(Transform target)
        {
            SetTarget(target.position);
        }

        public void SetTarget(Vector2 targetPosition)
        {
            Vector2 fromPosition = transform.position;
            TargetPosition = targetPosition;
            PathRequestManager.RequestPath(LevelManager.Instance.Grid, fromPosition, targetPosition , SetPathToTarget);
        }

        private void SetPathToTarget(List<Vector2> waypoints, bool success)
        {
            if (this == null)
            {
                return;
            }
            if (success)
            {
                Stop();
                _waypoints = waypoints;
                StartCoroutine(nameof(FollowPath));
            }
            else
            {
                OnSetTargetFailed?.Invoke();
            }
        }

        private IEnumerator FollowPath()
        {
            Moving = true;
            if (_waypoints != null && _waypoints.Count > 0)
            {
                Vector2 currentWaypoint = _waypoints[0];
                int targetIndex = 0;

                while (true)
                {
                    Vector2 position = transform.position;
                    if (Vector2.Distance(position, currentWaypoint) < 0.1f)
                    {
                        targetIndex++;
                        if (targetIndex >= _waypoints.Count)
                        {
                            Moving = false;
                            OnTargetReached?.Invoke();
                            yield break;
                        }

                        currentWaypoint = _waypoints[targetIndex];
                    }

                    //Debug.Log($"position: {position} - target: {currentWaypoint} - speed: {_speed * Time.deltaTime}");

                    Vector2 nextPos = Vector2.MoveTowards(position, currentWaypoint, _speed * Time.deltaTime);
                    transform.position = nextPos;
                    yield return null;
                }
            }

            Moving = false;
        }

        private void OnDrawGizmos()
        {
            if (_waypoints != null)
            {
                foreach (var pos in _waypoints)
                {
                    Gizmos.DrawSphere(pos, 2);
                }
            }
        }
    }
}