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
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;

        private float  TimeSinceLastAttack = 0f;
        private ActionScheduler actionScheduler;
        private Mover mover;

        private void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
        }
        private void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;
            if (target != null)
            {
               //  mover.MoveWithinRange(target.position, weaponRange);
                if (GetIsInRange())
                {
                    AttackBehavior();
                    mover.Cancel();
                }
                else
                {
                    mover.MoveTo(target.position);
                }
            }
        }

        private void AttackBehavior()
        {
            if (TimeSinceLastAttack >= timeBetweenAttacks)
            { 
                GetComponent<Animator>().SetTrigger("attack");
                TimeSinceLastAttack = 0f;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
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
        //Animation Event
         private void Hit ()
        {
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }
    }

}

