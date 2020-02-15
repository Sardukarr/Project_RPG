using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;
        public void Spawn(float dmg)
        {
            //damageTextPrefab has self destroy component
            DamageText instance = Instantiate(damageTextPrefab,transform);
            instance.SetText(dmg);
        }
    }
}