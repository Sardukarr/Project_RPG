using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            var waypoints = GetComponentsInChildren<Transform>();
            
            for(int i=0;i<transform.childCount ;i++)
            //foreach (var waypoint in waypoints)
            {
                Gizmos.DrawSphere(GetWaypoint(i), 0.2f);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetWaypointIndex(i)));
            }
        }

        public int GetWaypointIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
