using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction , ISaveable
    {

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
       // [SerializeField] string defaultWeaponName = "Unarmed";
        private float  TimeSinceLastAttack = 0f;

        private Health target;
        private ActionScheduler actionScheduler;
        private Mover mover;
        //TODO:: animation has to be for comercial use
        private Animator animator;

        Weapon currentWeapon=null;

        private void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            // name of resources has to be unique to make this work ( multiple resources folder)
            // Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
            //  EquipWeapon(weapon);
            if(currentWeapon == null)
                EquipWeapon(defaultWeapon);
        }


        private void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;
            if (!CanAttack()) return;
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
        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());

        }
        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
        if (TimeSinceLastAttack >= currentWeapon.TimeBetweenAttacks && !target.IsDead())
            {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                TimeSinceLastAttack = 0f;
            }
        }

        private bool IsInRange()
        {
            return target != null && (Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.Range);
        }
        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>() ;
            
        }
        public bool CanAttack()
        {
            return target != null && !target.IsDead();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            return combatTarget != null && 
                    combatTarget.GetComponent<Health>()!=null &&
                    !combatTarget.GetComponent<Health>().IsDead();
        }

        public Health GetTarget()
        {
            return target;
        }

        public void Cancel()
        {
            animator.SetTrigger("stopAttack");
            mover.Cancel();
            target = null;
        }
        //Animation Events
         private void Hit ()
        {
            if (currentWeapon.HasProjectile())
                Shoot();
            else if (target!=null & IsInRange())
                target.TakeDamage(gameObject, currentWeapon.Damage);
        }
        private void Shoot ()
        {
            if (target == null || !IsInRange()) return;

            currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            //Hit();
        }

        public object CaptureState()
        {
            if (currentWeapon != null)
                return currentWeapon.name;
            else return defaultWeapon.name;
        }

        public void RestoreState(object state)
        {
            //string WeaponName =(string) state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }
    }

}

