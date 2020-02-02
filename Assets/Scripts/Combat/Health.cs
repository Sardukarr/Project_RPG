using UnityEngine;
using System.Collections;

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
        if (healthPoints == 0&& !alreadyDead)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<Animator>().SetTrigger("die");
        alreadyDead = true;
    }
}
