using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, iAction
    {
        NavMeshAgent myAgent;
        ActionScheduler actionScheduler;
        // Start is called before the first frame update
        void Start()
        {
            myAgent = GetComponent<NavMeshAgent>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        // Update is called once per frame
        void Update()
        {
            //if(Input.GetMouseButton(0))
            //{
            //    MoveToCursor();  
            //}
            updateAnimator();
        }

        private void updateAnimator()
        {
            Vector3 velocity = myAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        public void StartMoveAction(Vector3 destination)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }
        public void Cancel()
        {
            //myAgent.SetDestination(transform.position);
            myAgent.isStopped = true;
        }
        public void MoveTo(Vector3 destination)
        {
            myAgent.isStopped = false;
            myAgent.destination = destination;

        }
        public void MoveWithinRange(Vector3 destination, float distance)
        {
          //  myAgent.destination = destination;
            myAgent.SetDestination(destination);
            var remainingDistance = Vector3.Distance(transform.position, destination);
            //Vector3.Distance(transform.position, destination);
            if (remainingDistance <= distance)
                Cancel();      
            else
            {
                myAgent.isStopped = false;
            }

        }
    }

}