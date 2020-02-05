using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon",menuName ="Weapons/Make New Weapon",order =0)]
  public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController AnimatorOverride = null;
        [SerializeField] float range = 0.5f;
        [SerializeField] float damage = 1f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        public float TimeBetweenAttacks { get => timeBetweenAttacks; set => timeBetweenAttacks = value; }
        public float Damage { get => damage; set => damage = value; }
        public float Range { get => range; set => range = value; }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {

            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Instantiate(equippedPrefab, handTransform);
            }
            if (AnimatorOverride != null)
                animator.runtimeAnimatorController = AnimatorOverride;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,damage);
        }
    }
}