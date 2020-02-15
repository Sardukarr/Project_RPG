using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction , ISaveable, IModifierProvider
    {

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
       // [SerializeField] string defaultWeaponName = "Unarmed";
        private float  TimeSinceLastAttack = 0f;

        private Health target = null;
        private ActionScheduler actionScheduler = null;
        private Mover mover = null;
        //TODO:: animation has to be for comercial use
        private Animator animator = null;
        private BaseStats baseStats = null;

        LazyValue<WeaponConfig> currentWeaponConfig=null;
        Weapon currentWeapon = null;
        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            currentWeaponConfig = new LazyValue<WeaponConfig>(SetupDefaultWeapon);

        }

        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start()
        {
            currentWeaponConfig.ForceInit();
            
        }
        public bool hasValidTarget()
        {
            return target != null && CanAttack(target.gameObject);
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
        public void EquipWeapon(WeaponConfig weapon)
        {
            if (weapon == null) return;
            currentWeaponConfig.value = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            // Race condition, used in start and restore state, can not use local stashed animator
            animator.SetFloat("attackSpeedMultiplayer", currentWeaponConfig.value.GetAnimationSpeed()); 

        }
        private void AttachWeapon(WeaponConfig weapon)
        {
            if (weapon == null) return;
            currentWeaponConfig.value = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            animator.SetFloat("attackSpeedMultiplayer", currentWeaponConfig.value.GetAnimationSpeed());

        }
        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
        if (TimeSinceLastAttack >= currentWeaponConfig.value.TimeBetweenAttacks && !target.IsDead())
            {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
              //  animator.speed = currentWeapon.GetAnimationSpeed();
                TimeSinceLastAttack = 0f;
            }
        }

        private bool IsInRange()
        {
            return target != null && (Vector3.Distance(transform.position, target.transform.position) <= currentWeaponConfig.value.Range);
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
            if (currentWeaponConfig.value.HasProjectile())
                Shoot();
            else if (target != null & IsInRange())
                //          target.TakeDamage(gameObject, currentWeapon.Damage);
                target.TakeDamage(gameObject, (float)baseStats.GetStat(Stat.BaseDmg));//+currentWeapon.Damage);
        }
        private void Shoot ()
        {
            if (target == null) return; //|| !IsInRange()) return;

            currentWeaponConfig.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, (float)baseStats.GetStat(Stat.BaseDmg)); //+ currentWeapon.Damage);
            //Hit();
        }

        public object CaptureState()
        {
            if (currentWeaponConfig != null)
                return currentWeaponConfig.value.name;
            else return defaultWeapon.name;
        }

        public void RestoreState(object state)
        {
            //string WeaponName =(string) state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>((string)state);
            target = null;
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            //TODO: should dmg be int?
            if(stat == Stat.BaseDmg)
            {
                yield return currentWeaponConfig.value.Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.BaseDmg)
            {
                yield return currentWeaponConfig.value.PercentageModifier;
            }
        }
    }

}

