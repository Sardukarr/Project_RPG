using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;
using RPG.Attributes;
using System;
using RPG.Enviroment;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] float maxSpeed=6f;
        [SerializeField] float MaxNavMeshPathLength = 40f;
        NavMeshAgent myAgent;
        ActionScheduler actionScheduler;
        Health myHealth;

        void Start()
        {
            myAgent = GetComponent<NavMeshAgent>();
            actionScheduler = GetComponent<ActionScheduler>();
            myHealth = GetComponent<Health>();
        }
        void Update()
        {
            myAgent.enabled = !myHealth.IsDead();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = myAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
          //  target = navMeshHit.position;
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > MaxNavMeshPathLength) return false;

            return true;
        }
       
        public void Cancel()
        {
            myAgent.isStopped = true;
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            myAgent.isStopped = false;
            myAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            myAgent.destination = destination;

        }
        public void MoveWithinRange(Vector3 destination, float distance)
        {
            myAgent.SetDestination(destination);
            actionScheduler.StartAction(this);
            var remainingDistance = Vector3.Distance(transform.position, destination);
            if (remainingDistance <= distance)
            {
                actionScheduler.CancelAction();
            }
            else
            {
                myAgent.isStopped = false;
            }

        }
        private float GetPathLength(NavMeshPath path)
        {
            Vector3 lastCorner = transform.position;
            float Distance = 0f;
            for (int i = 0; i < path.corners.Length; i++)
            {
                Distance += Vector3.Distance(lastCorner, path.corners[i]);
                lastCorner = path.corners[i];
            }
            //  print(Distance);      
            return Distance;
        }
        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            var data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }

        private void FootL()
        {
            //basicly a stepo meter
           // print("footl");
        }
        private void FootR()
        {
          //  print("footr");
        }
    }

}