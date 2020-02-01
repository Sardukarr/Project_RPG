using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent MyAgent;
    // Start is called before the first frame update
    void Start()
    {
        MyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveToCursor();  
        }
        updateAnimator(); 
    }

    private void updateAnimator()
    {
        Vector3 velocity = MyAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
            MyAgent.destination = hit.point;

        Debug.DrawRay(ray.origin, ray.direction * 100);
    }
}
