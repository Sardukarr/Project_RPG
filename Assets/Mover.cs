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
