using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float shoutDistance = 8f;
        [SerializeField] float SuspecionTime = 5f;
        [SerializeField] float waypointDwellTime = 5f;
        [SerializeField] float waypointTolerance = 1.2f;
        [SerializeField] float AggrevatedTime = 10f;
        [Range(0,1)][SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] PatrolPath myPatrolPath=null;

        private float timeSinceAggrevated = Mathf.Infinity;
        private float timeSincePlayerSpotted = Mathf.Infinity;
        private float timeSinceArrivedOnWaypoint = Mathf.Infinity;

        private GameObject player;
        private Mover myMover;
        private Fighter myFighter;
        private ActionScheduler mySchedule;
        private Health myHealth;
      //  private NavMeshAgent myAgent;
        private int CurrentWaypointIndex=0;
        LazyValue<Vector3> guardPosition;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(Config.playerTag);
            myMover = GetComponent<Mover>();
            myHealth = GetComponent<Health>();
            myFighter = GetComponent<Fighter>();
            mySchedule = GetComponent<ActionScheduler>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
           // myAgent = GetComponent<NavMeshAgent>();
          //  guardPosition = transform.position;
         //   startSpeed = myAgent.speed;
        }
        private void Update()
        {
            if (myHealth.IsDead())
                return;
            UpdateTimers();
            if (IsAggrevated() )
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

        public void TurnAggressionOn()
        {
            timeSinceAggrevated = 0f;
        }
        private void GuardBehaviour()
        {
            mySchedule.CancelAction();
           // myAgent.speed = startSpeed;
            myMover.StartMoveAction(guardPosition.value,patrolSpeedFraction);
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
            AggrivateNearbyEnamies();
        }
        private void AggrivateNearbyEnamies()
        {
            var hits =Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0f);
            foreach (var hit in hits)
            {
               var fellowEnamy = hit.transform.GetComponent<AIController>();
                if(fellowEnamy!=null)
                {
                    fellowEnamy.TurnAggressionOn();
                }
            }
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
            if(waypointDwellTime < timeSinceArrivedOnWaypoint)
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
            timeSinceAggrevated += Time.deltaTime;
        }
        private bool IsAggrevated()
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            
            return ((distance <= chaseDistance ) || (timeSinceAggrevated < AggrevatedTime)) &&
                  myFighter.CanAttack(player);
        }

        //called by unity editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}