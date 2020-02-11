using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {

        float healthPoints = -1f;
        float maxHealthPoints = -1f;
        public bool alreadyDead = false;
        BaseStats baseStats;

        public void Start()
        {
            baseStats = GetComponent<BaseStats>();
            //    GetComponent<Animator>().ResetTrigger("resurect");
            maxHealthPoints = baseStats.GetStat(Stat.HP);
            if (healthPoints < 0f)
            {
                healthPoints = maxHealthPoints;
            }

            GetComponent<BaseStats>().onLevelUp += HealOnLevelUp;
        }

        private void HealOnLevelUp()
        {
            var NewMaxHp = baseStats.GetStat(Stat.HP);
            healthPoints += Mathf.Clamp(NewMaxHp-maxHealthPoints, healthPoints,maxHealthPoints);
            maxHealthPoints = NewMaxHp;
        }

        public bool IsDead()
        {
            return alreadyDead;
        }
        // public bool AlreadyDead { get => alreadyDead; private set => alreadyDead = value; }

        public void TakeDamage(GameObject instigator ,float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0 && !alreadyDead)
            {
                Die(instigator);
            }
            else
            {
                //GetComponent<Animator>().st
                GetComponent<Animator>().SetTrigger("getHit");
            }
        }
        public float GetPercent()
        {
            return 100*healthPoints / baseStats.GetStat(Stat.HP);
        }
        private void Die(GameObject instigator)
        {
            // if (alreadyDead) return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
            alreadyDead = true;
            if(instigator!=null)
                instigator.GetComponent<Experience>().AwardExp(baseStats.GetStat(Stat.XPReward));
        }
        [System.Serializable]
        struct HealthSaveData
        {
            public float healthPoints;
            public bool alreadyDead;
        }

         
        public object CaptureState()
        {
            HealthSaveData data = new HealthSaveData();
            data.healthPoints = healthPoints;
            data.alreadyDead = alreadyDead;
            return data;
        }

        public void RestoreState(object state)
        {
            HealthSaveData data = (HealthSaveData)state;
            healthPoints = data.healthPoints;
            alreadyDead = data.alreadyDead;
           
            if (healthPoints > 0)
            {
                if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
                    GetComponent<Animator>().SetTrigger("resurect");
            

            }
            else
                Die(null);
           // GetComponent<Animator>().ResetTrigger("resurect");
        }

    }
}
