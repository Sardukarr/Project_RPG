using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed=6f;
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
            var remainingDistance = Vector3.Distance(transform.position, destination);
            if (remainingDistance <= distance)
                Cancel();      
            else
            {
                myAgent.isStopped = false;
            }

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
    }

}