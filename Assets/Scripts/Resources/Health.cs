﻿using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        //lazy value would be initialize before first use;
        LazyValue<float> healthPoints;
        LazyValue<float> maxHealthPoints;
        public bool alreadyDead = false;
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            // maxHealthPoints.value = baseStats.GetStat(Stat.HP);
            //lazy value would be initialize before first use;

            healthPoints = new LazyValue<float>(GetInitialHealth);
            maxHealthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HP);
        }
        public void Start()
        {
            healthPoints.ForceInit();
            maxHealthPoints.ForceInit();
            //    GetComponent<Animator>().ResetTrigger("resurect");

        }
        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += HealOnLevelUp;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= HealOnLevelUp;
        }
        private void HealOnLevelUp()
        {
            float NewMaxHp = baseStats.GetStat(Stat.HP);
            float newhealthPoints = healthPoints.value + NewMaxHp - maxHealthPoints.value;
            // float newhealthPoints = healthPoints+ (GetPercent()/100 * NewMaxHp);
            healthPoints.value = Mathf.Clamp(newhealthPoints, healthPoints.value, NewMaxHp);

            maxHealthPoints.value = NewMaxHp;
        }

        public bool IsDead()
        {
            return alreadyDead;
        }
        // public bool AlreadyDead { get => alreadyDead; private set => alreadyDead = value; }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "took: " + damage);
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (healthPoints.value == 0 && !alreadyDead)
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
            return 100 * healthPoints.value / baseStats.GetStat(Stat.HP);
        }
        public Tuple<float, float> GetCurrentAndMaxHealth()
        {
            return new Tuple<float, float>(healthPoints.value, maxHealthPoints.value);
        }
        private void Die(GameObject instigator)
        {
            // if (alreadyDead) return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
            alreadyDead = true;
            if (instigator != null)
                instigator.GetComponent<Experience>().AwardExp((int)baseStats.GetStat(Stat.XPReward));
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
            data.healthPoints = healthPoints.value;
            data.alreadyDead = alreadyDead;
            return data;
        }

        public void RestoreState(object state)
        {
            HealthSaveData data = (HealthSaveData)state;
            healthPoints.value = data.healthPoints;
            alreadyDead = data.alreadyDead;

            if (healthPoints.value > 0)
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
