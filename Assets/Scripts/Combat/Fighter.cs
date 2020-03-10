using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

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
        WeaponConfig currentWeaponConfig= null;
        LazyValue<Weapon> currentWeapon = null;

        Equipment equipment=null;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }

        }

        private Weapon SetupDefaultWeapon()
        {

            return AttachWeapon(defaultWeapon); 
        }

        private void Start()
        {
            currentWeapon.ForceInit();
            
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
            currentWeaponConfig = weapon;
            currentWeapon.value = weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            // Race condition, used in start and restore state, can not use local stashed animator
            animator.SetFloat("attackSpeedMultiplayer", currentWeaponConfig.GetAnimationSpeed()); 

        }
        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            if (weapon == null) return null;
            currentWeaponConfig = weapon;
            currentWeapon.value = weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            animator.SetFloat("attackSpeedMultiplayer", currentWeaponConfig.GetAnimationSpeed());
            return currentWeapon.value;
        }
        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }
        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
        if (TimeSinceLastAttack >= currentWeaponConfig.TimeBetweenAttacks && !target.IsDead())
            {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
              //  animator.speed = currentWeapon.GetAnimationSpeed();
                TimeSinceLastAttack = 0f;
            }
        }

        private bool IsInRange()
        {
            return target != null && (Vector3.Distance(transform.position, target.transform.position) <= currentWeaponConfig.Range);
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
           // if (combatTarget == null) return false;
           // if (!mover.CanMoveTo(combatTarget.transform.position)) return false;
            var targetHealth = combatTarget.GetComponent<Health>();
            return combatTarget != null &&
                    targetHealth != null &&
                    !targetHealth.IsDead() &&
                    (mover.CanMoveTo(combatTarget.transform.position) || currentWeaponConfig.HasProjectile())
                    ;
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
        //Animation Event
         private void Hit ()
        {
            if (currentWeapon.value != null)
                currentWeapon.value.OnHit();
            if (currentWeaponConfig.HasProjectile())
                LaunchProjectile();
            else if (target != null & IsInRange())
                //          target.TakeDamage(gameObject, currentWeapon.Damage);
                target.TakeDamage(gameObject, (float)baseStats.GetStat(Stat.BaseDmg));//+currentWeapon.Damage);
        }
        //Animation Event
        void Shoot()
        {
            Hit();
        }
        private void LaunchProjectile ()
        {
            if (target == null) return; //|| !IsInRange()) return;

            currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, (float)baseStats.GetStat(Stat.BaseDmg)); //+ currentWeapon.Damage);
            //Hit();
        }

        public object CaptureState()
        {
            if (currentWeaponConfig != null)
                return currentWeaponConfig.name;
            else return defaultWeapon.name;
        }

        public void RestoreState(object state)
        {
            //string WeaponName =(string) state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>((string)state);
            target = null;
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            //TODO: should dmg be int?
            if(stat == Stat.BaseDmg)
            {
                yield return currentWeaponConfig.Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.BaseDmg)
            {
                yield return currentWeaponConfig.PercentageModifier;
            }
        }
    }

}

