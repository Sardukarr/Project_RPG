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
        [SerializeField] GameObject impactFX = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 0.05f;

        bool homing = false;
        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
           if(homing && !target.IsDead())
                transform.LookAt(target.transform.position);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target,float WeaponDamage, bool homing=false)
        {
            this.target = target;
            this.damage += WeaponDamage;
            this.homing = homing;
            transform.LookAt(target.transform.position);
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
            if (ColiderHealth != null) // return; ?
            {
            //TODO maybe disable friendly fire
            //   if (ColiderHealth != target) return;
                if (!ColiderHealth.IsDead())
                {
                ColiderHealth.TakeDamage(damage); // targer.takedamage
                }
                else return;
            }
            if (impactFX != null)
            {
                GameObject impact = Instantiate(impactFX, other.GetComponent<Transform>().position, transform.rotation);
              //  Destroy(impact, 2f);
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}