using RPG.Core;
using System;
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
        [SerializeField] Progression progression=null;
        [SerializeField] GameObject levelUpParticle = null;
        Experience experience = null;

        public event Action onLevelUp;
        //    [SerializeField] Stats stat;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            experience = GetComponent<Experience>();
            if (experience != null)
                experience.onExperienceGained += UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel>currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticle, transform);
        }

        public int GetStat(Stat stat)
        {
            return progression.GetStat(stat,characterClass,GetLevel()); 
        }
        public int GetLevel()
        {
            if (currentLevel < 1)
                currentLevel = CalculateLevel();
            return currentLevel;
        }
        public int CalculateLevel()
        {
            experience = GetComponent<Experience>();
            if (experience == null) return currentLevel;

            int currentXP = experience.GetXP();
            int maxLevel = progression.GetLevels(Stat.XPToLevelUp, characterClass);
            for (int levels=1; levels <= maxLevel; levels++)
            {
                var XpToLvlUp = progression.GetStat(Stat.XPToLevelUp, characterClass, levels);
                if (XpToLvlUp > currentXP)
                    return levels;
            }
            return maxLevel+1;
        }
    }

}