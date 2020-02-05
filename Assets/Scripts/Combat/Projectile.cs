using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] Health target = null;
        [SerializeField] float speed = 8f;
        [SerializeField] float damage = 0f;
        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            transform.LookAt(target.transform.position);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target,float WeaponDamage)
        {
            this.target = target;
            this.damage += WeaponDamage;
        }
        //for target that has capsule, and pivot near ground
        // currently not used
        private Vector3 GetAimLocation()
        {
            return target.GetComponent<CapsuleCollider>().center;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health ColiderHealth = other.GetComponent<Health>();
            if (ColiderHealth != null)
            {
                //TODO maybe disable friendly fire
                if (ColiderHealth != target) return;
                target.TakeDamage(damage); // targer.takedamage
            }
            Destroy(gameObject);
        }
    }
}