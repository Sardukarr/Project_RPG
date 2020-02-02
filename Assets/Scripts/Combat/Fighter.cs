using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, iAction
    {
        Transform target;

        [SerializeField] float weaponRange = 1f;
        private ActionScheduler actionScheduler;
        private Mover mover;

        private void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
        }
        private void Update()
        {
            if (target != null)
            {
                 mover.MoveWithinRange(target.position, weaponRange);
                //if (!GetIsInRange())
                //{
                //    mover.MoveTo(target.position);
               // }
              //  else
              //      mover.Cancel();
            }
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }
        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }
        public void Cancel()
        {
            target = null;
        }
    }

}

