using UnityEngine;
using System.Collections;
using RPG.Saving;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable

    {

        [SerializeField]float XP;

        public void AwardExp(float XP)
        {
            this.XP += XP; 
        }
        public float GetXP()
        {
            return XP;
        }
        public object CaptureState()
        {
            return XP;
        }

        public void RestoreState(object state)
        {
            XP = (float)state;
        }
    }
}