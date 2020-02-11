using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable

    {

        [SerializeField]int XP;

        //public delegate void ExperienceGainedDelegate();
        public event Action onExperienceGained;

        public void AwardExp(int XP)
        {
            this.XP += XP;
            onExperienceGained();
        }
        public int GetXP()
        {
            return XP;
        }
        public object CaptureState()
        {
            return XP;
        }

        public void RestoreState(object state)
        {
            XP = (int)state;
        }
    }
}