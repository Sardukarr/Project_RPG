using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, iAction
    {


        [SerializeField] float weaponRange = 1f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;


        private float  TimeSinceLastAttack = 0f;

        private Health target;
        private ActionScheduler actionScheduler;
        private Mover mover;
        //TODO:: animation has to be for comercial use
        private Animator animator;



        private void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }
        private void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;
            if (!canAttack()) return;
               //  mover.MoveWithinRange(target.position, weaponRange);
                if (IsInRange())
                {
                    AttackBehavior();
                    mover.Cancel();
                }
                else
                {
                    mover.MoveTo(target.transform.position,1f);
                }
            
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
        if (TimeSinceLastAttack >= timeBetweenAttacks && !target.IsDead())
            {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                TimeSinceLastAttack = 0f;
            }
        }

        private bool IsInRange()
        {
            return target != null && (Vector3.Distance(transform.position, target.transform.position) <= weaponRange);
        }
        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>() ;
            
        }
        public bool canAttack()
        {
            return target != null && !target.IsDead();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            return combatTarget != null && 
                    combatTarget.GetComponent<Health>()!=null &&
                    !combatTarget.GetComponent<Health>().IsDead();
        }
        public void Cancel()
        {
            animator.SetTrigger("stopAttack");
            target = null;
        }
        //Animation Event
         private void Hit ()
        {
            if(target!=null & IsInRange())
                target.TakeDamage(weaponDamage);
        }
    }

}

