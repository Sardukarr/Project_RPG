using GameDevTV.Utils;
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

        private LazyValue<int> currentLevel;

        //dont call external function on awake
        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue <int> (GetInitLevel);
        }

        private int GetInitLevel()
        {
           return CalculateLevel();
        }

        private void Start()
        {
            //currentLevel = CalculateLevel();
            currentLevel.ForceInit();
           
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel>currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }
        // good practice to subscribe to delegates in onEnable (awake< here < start
        private void OnEnable()
        {
            if (experience != null)
                experience.onExperienceGained += UpdateLevel;
        }
        private void OnDisable()
        {
            if (experience != null)
                experience.onExperienceGained -= UpdateLevel;
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
            return currentLevel.value;
        }
        public int CalculateLevel()
        {
            //TODO: could be experience from awake?
            experience = GetComponent<Experience>();
            if (experience == null) return currentLevel.value;

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