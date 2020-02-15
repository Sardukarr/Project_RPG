using RPG.Core;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon",menuName ="Weapons/Make New Weapon",order =1)]
  public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] AnimatorOverrideController AnimatorOverride = null;
        [SerializeField] float range = 0.5f;
        [SerializeField] float damage = 1f;
        [SerializeField] float percentageModifier = 0f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        [SerializeField] bool homing = false;
        [SerializeField] float animationSpeed = 1f;

        public float TimeBetweenAttacks { get => timeBetweenAttacks; set => timeBetweenAttacks = value; }
        public float Damage { get => damage; set => damage = value; }
        public float Range { get => range; set => range = value; }
        public float PercentageModifier { get => percentageModifier; set => percentageModifier = value; }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                //handTransform = GetTransform(rightHand, leftHand);
                Weapon weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = Config.weaponName;
            }
            // magic to override back to default when new weapon has no ovveride controler 
            //(it would use last weapon animation if not set properly)
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (AnimatorOverride != null)
                animator.runtimeAnimatorController = AnimatorOverride;
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldOne = rightHand.Find(Config.weaponName);
            if (oldOne ==null)
                oldOne = leftHand.Find(Config.weaponName);
            if (oldOne == null) return;
            oldOne.name = "DESTROYING"; // prevent destroing new pickup
            Destroy(oldOne.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            // ToDO how to archer not kill himself!
           // Vector3 test = isRightHanded ? rightHand.position + (rightHand.forward) : leftHand.position + (leftHand.forward);
             //   test = test + test.

            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public float GetAnimationSpeed()
        {
            return animationSpeed;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator,float CalculatedDmg)
        {
            // 1.5f magic distance when arrow dont hit archer
            Transform ProjectileStartPoint = GetTransform(rightHand, leftHand);
            // ProjectileStartPoint is in fact a hand transform, comes as an reference
           // ProjectileStartPoint.position -= ProjectileStartPoint.right/8f;
            Projectile projectileInstance = Instantiate(projectile, ProjectileStartPoint.position- ProjectileStartPoint.right / 8f, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, CalculatedDmg, homing);
        }

        
    }
}