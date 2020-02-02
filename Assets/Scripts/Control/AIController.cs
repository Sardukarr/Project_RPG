using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float SuspecionTime = 5f;
        [SerializeField] float waypointDwellTime = 5f;
        [SerializeField] float waypointTolerance = 1.2f;
        [SerializeField] PatrolPath myPatrolPath;
 

        float timeSincePlayerSpotted= Mathf.Infinity;
        float timeSinceArrivedOnWaypoint = Mathf.Infinity;

        private GameObject player;
        private Mover myMover;
        private Fighter myFighter;
        private ActionScheduler mySchedule;
        private Health myHealth;
        private int CurrentWaypointIndex=0;



        Vector3 guardPosition;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            myMover = GetComponent<Mover>();
            myHealth = GetComponent<Health>();
            myFighter = GetComponent<Fighter>();
            mySchedule = GetComponent<ActionScheduler>();
            guardPosition = transform.position;
        }
        private void Update()
        {
            if (myHealth.IsDead())
                return;
            UpdateTimers();
            if (DistanceToPlayer() <= chaseDistance && myFighter.CanAttack(player))
            {
                AttackBehaviour();

            }
            else if (SuspecionTime > timeSincePlayerSpotted)
            {
                SuspecionBehaviour();
            }
            else
            {
                if (myPatrolPath != null)
                    PatrolBehaviour();
                else
                    GuardBehaviour();
            }

        }

        private void GuardBehaviour()
        {
            mySchedule.CancelAction();
            myMover.StartMoveAction(guardPosition);
        }

        private void SuspecionBehaviour()
        {
            mySchedule.CancelAction();
        }

        private void AttackBehaviour()
        {
            timeSincePlayerSpotted = 0;
            myFighter.Attack(player);
        }

        private void PatrolBehaviour()
        {
            
            if (AtWaypoint())
            {
                timeSinceArrivedOnWaypoint = 0;
                CycleWaypoint();
            }
            Vector3 desiredPosition = GetCurrentWaypoint();
            if(waypointDwellTime<timeSinceArrivedOnWaypoint)
                myMover.StartMoveAction(desiredPosition);

        }

        private Vector3 GetCurrentWaypoint()
        {
            return myPatrolPath.GetWaypoint(CurrentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            
            CurrentWaypointIndex = myPatrolPath.GetWaypointIndex(CurrentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }
        private void UpdateTimers()
        {
            timeSincePlayerSpotted += Time.deltaTime;
            timeSinceArrivedOnWaypoint += Time.deltaTime;
        }
        private float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        //called by unity editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}