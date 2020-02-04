using UnityEngine;
using UnityEngine.Playables;
using RPG.Control;
using RPG.Core;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindGameObjectWithTag("Player");

        }
        void DisableControl(PlayableDirector aDirector)
        {
            
            player.GetComponent<ActionScheduler>().CancelAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
        void EnableControl(PlayableDirector aDirector)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
