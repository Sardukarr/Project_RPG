using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control
{ 
    public class PlayerController : MonoBehaviour
    {
        Mover myMover;
        Fighter myFighter;
        private Health myHealth;

        void Start()
        {
            myMover = GetComponent<Mover>();
            myFighter = GetComponent<Fighter>();
            myHealth = GetComponent<Health>();
        }
    
        void Update()
        {
            if (myHealth.IsDead()) return;
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
                if (target == null) continue;
                if (!myFighter.CanAttack(target.gameObject)) continue;
                    myFighter.Attack(target.gameObject);
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
                myMover.StartMoveAction(hit.point,1f);
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