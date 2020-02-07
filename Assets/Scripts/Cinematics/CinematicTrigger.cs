using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadyTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggered && other.gameObject.CompareTag(Config.playerTag))
            { 
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;
            }
          
        }
    }

}