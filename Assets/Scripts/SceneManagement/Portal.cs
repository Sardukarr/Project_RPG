using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            North,
            East,
            South,
            West,
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier location;
        private DestinationIdentifier destination;
        private void Start()
        {
            //  spawnPoint = GetComponentInChildren<Transform>();
            destination = (DestinationIdentifier)(((int)location + 2)%2);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag=="Player")
            {
                //  SceneManager.LoadScene(sceneToLoad);
                StartCoroutine(Transition());

            }
        }
        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            Portal otherPortal = GetOtherPortal(destination);
            updatePlayer(otherPortal);
            Destroy(gameObject);
        }

        private void updatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            //  NavMeshAgent might try to update pos at the same time
           
            if (otherPortal!=null)
            {
                player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            }
           // player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal(DestinationIdentifier CurrentLocalisation)
        {
            Portal[] portals = FindObjectsOfType<Portal>();

         // TODO ensure that always south -  north, west - east is present at scene
            foreach ( Portal portal in portals)
            {
                
                if (portal == this) continue;
                if(portal.destination == CurrentLocalisation)
                return portal;
            }
            return null;
        }
    }
}