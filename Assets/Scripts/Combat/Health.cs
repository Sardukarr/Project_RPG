using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

   [SerializeField] float currentHealth = 100f;
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage,0);
        print(currentHealth);
    }
}
