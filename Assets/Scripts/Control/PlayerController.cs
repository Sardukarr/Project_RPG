using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;

namespace RPG.Control
{ 
    public class PlayerController : MonoBehaviour
    {
        Mover myMover;
        Fighter myFighter;
        void Start()
        {
            myMover = GetComponent<Mover>();
            myFighter = GetComponent<Fighter>();
        }
    
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (InteractWithCombat()) return;
                if (InteractWithMovement()) return;
                print("nothing to do here");
            }

        }

        private bool InteractWithCombat()
        {
            bool isThereTargetToAttack = false;
            RaycastHit[] hits = Physics.RaycastAll(GetMauseRey());
                foreach (var hit in hits)
                {
                    var target = hit.transform.GetComponent<CombatTarget>();
                if (!myFighter.canAttack(target)) continue;
                    myFighter.Attack(target);
                    isThereTargetToAttack = true;
                }
          if(!isThereTargetToAttack)
                myFighter.Cancel();
            return isThereTargetToAttack;
        }

        private bool InteractWithMovement()
        {
            bool isThereValidPlaceToGo=false;
            if (Physics.Raycast(GetMauseRey(), out RaycastHit hit))
            {
                myMover.StartMoveAction(hit.point);
                isThereValidPlaceToGo = true;
            }
            return isThereValidPlaceToGo;
        }

        private static Ray GetMauseRey()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}