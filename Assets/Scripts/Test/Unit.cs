using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using PathFinding.AStar;
using UnityEngine;
using Util;
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
            _speed = Random.Range(10.0f, 15.0f);
        }


        public void SetTarget(LevelAStarGrid aStarGrid, Transform target)
        {
            _target = target;
            Vector2 from = transform.position;
            Vector2 to = target.position;
            PathRequestManager.RequestPath(aStarGrid, from, to , SetPathToTarget);
        }

        private void SetPathToTarget(List<Vector2> waypoints, bool success)
        {
            if (success)
            {
                _waypoints = waypoints;
                StopCoroutine(FollowPath());
                StartCoroutine(FollowPath());
            }
            else
            {
                Destroy(gameObject);
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
                        Destroy(gameObject);
                        yield break;
                    }
                    currentWaypoint = _waypoints[targetIndex];
                }

                //Debug.Log($"{position} -> {currentWaypoint}");
                Vector2 nextPos = Vector2.MoveTowards(position, currentWaypoint, _speed*Time.deltaTime);
                transform.position = nextPos;
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (_waypoints == null) return;
            Gizmos.color = Color.magenta;
            foreach (var wp in _waypoints)
            {
                Vector3 pos = wp;
                
                Gizmos.DrawSphere(pos, 1);
            }
        }
    }
}