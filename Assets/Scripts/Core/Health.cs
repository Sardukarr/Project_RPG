using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {

        [SerializeField] float healthPoints = 100f;
        public bool alreadyDead = false;

        public bool IsDead()
        {
            return alreadyDead;
        }
        // public bool AlreadyDead { get => alreadyDead; private set => alreadyDead = value; }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0 && !alreadyDead)
            {
                Die();
            }
        }

        private void Die()
        {
            if (alreadyDead) return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
            alreadyDead = true;
        }
    }
}
