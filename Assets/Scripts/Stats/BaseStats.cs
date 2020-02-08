using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 100)]
        [SerializeField] int level = 1;
        [SerializeField] Classes characterClass;
        [SerializeField] Progression progression;
    //    [SerializeField] Stats stat;
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat,characterClass,level); 
        }
    }

}