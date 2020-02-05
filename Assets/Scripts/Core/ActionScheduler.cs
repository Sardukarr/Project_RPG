﻿using UnityEngine;
using System.Collections;
namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel(); 
            }
            currentAction = action;
            
        }
        public void CancelAction()
        {
            StartAction(null);
        }
    }
}
