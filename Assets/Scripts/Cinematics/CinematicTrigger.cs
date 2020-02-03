using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadyTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggered)
            { 
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;
            }
          
        }
    }

}