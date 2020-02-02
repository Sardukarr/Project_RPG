using UnityEngine;
using System.Collections;
namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        iAction currentAction;
        public void StartAction(iAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                print("Cancelling" + currentAction);
                currentAction.Cancel(); 
            }
            currentAction = action;
            
        }
    }
}
