using RPG.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Make New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] CharacterClass[] characterClasses = null;
        Dictionary<Classes, Dictionary<Stat, float[]>> lookupTable = null;
        
        
        
        public float GetStat(Stat stat, Classes CharClass, int lvl )
        {

            Buildlookup();
            float[] levels =  lookupTable[CharClass][stat];
            if (levels.Length < lvl) return 1; // 
            else return levels[lvl-1];
        }

        private void Buildlookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<Classes, Dictionary<Stat, float[]>>();
            foreach (var progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (StatsClass progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }
        public float GetLevels(Stat stat, Classes CharClass)
        {
            Buildlookup();
            float[] levels = lookupTable[CharClass][stat];
            return levels.Length;
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