using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Resources;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float SuspecionTime = 5f;
        [SerializeField] float waypointDwellTime = 5f;
        [SerializeField] float waypointTolerance = 1.2f;
        [Range(0,1)][SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] PatrolPath myPatrolPath=null;


        private float timeSincePlayerSpotted = Mathf.Infinity;
        private float timeSinceArrivedOnWaypoint = Mathf.Infinity;

        private GameObject player;
        private Mover myMover;
        private Fighter myFighter;
        private ActionScheduler mySchedule;
        private Health myHealth;
      //  private NavMeshAgent myAgent;
        private int CurrentWaypointIndex=0;



        Vector3 guardPosition;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(Config.playerTag);
            myMover = GetComponent<Mover>();
            myHealth = GetComponent<Health>();
            myFighter = GetComponent<Fighter>();
            mySchedule = GetComponent<ActionScheduler>();
           // myAgent = GetComponent<NavMeshAgent>();
            guardPosition = transform.position;
         //   startSpeed = myAgent.speed;
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
           // myAgent.speed = startSpeed;
            myMover.StartMoveAction(guardPosition,patrolSpeedFraction);
        }

        private void SuspecionBehaviour()
        {
            mySchedule.CancelAction();
        }

        private void AttackBehaviour()
        {
            timeSincePlayerSpotted = 0;
          //  myAgent.speed = RunToPlayerSpeed;
            myFighter.Attack(player);
        }

        private void PatrolBehaviour()
        {
        //    myAgent.speed = startSpeed;
            if (AtWaypoint())
            {
                timeSinceArrivedOnWaypoint = 0;
                CycleWaypoint();
            }
            Vector3 desiredPosition = GetCurrentWaypoint();
            if(waypointDwellTime<timeSinceArrivedOnWaypoint)
                myMover.StartMoveAction(desiredPosition, patrolSpeedFraction);

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