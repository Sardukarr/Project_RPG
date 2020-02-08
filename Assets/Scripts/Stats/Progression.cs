using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Make New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] CharacterClass[] characterClasses = null;

        public float GetStat(Stat stat, Classes CharClass, int lvl )
        {
            foreach (var progressionClass in characterClasses)
            {
                if (progressionClass.characterClass != CharClass) continue;

                foreach (StatsClass progressionStat in progressionClass.stats)
                {
                    if (progressionStat.stat != stat) continue;
                    if (progressionStat.levels.Length < lvl) continue;
                    return progressionStat.levels[lvl - 1];
                }
                   
            }
            return 1f;
        }

        [System.Serializable]
        public class CharacterClass
        {
            public Classes characterClass;
            public StatsClass[] stats;
            
           
        }
        [System.Serializable]
        public class StatsClass
        {
            public Stat stat;
            public float[] levels;
        }
    }
}