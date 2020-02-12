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

        //TODO : should enamies get modifiers?
        // [SerializeField] bool shouldGetModifiers = true;

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

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1+GetPercentageModifier(stat)/100);
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
            int maxLevel = (int)progression.GetLevels(Stat.XPToLevelUp, characterClass);
            for (int levels=1; levels <= maxLevel; levels++)
            {
                int XpToLvlUp = (int)progression.GetStat(Stat.XPToLevelUp, characterClass, levels);
                if (XpToLvlUp > currentXP)
                    return levels;
            }
            return maxLevel+1;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
          foreach(var provider in GetComponents<IModifierProvider>())
            {
                foreach(var modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stat stat)
        {
            float total = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }
    }

}