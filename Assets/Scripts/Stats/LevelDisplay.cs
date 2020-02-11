using UnityEngine;
using RPG.Core;
using UnityEngine.UI;
using System;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats = null;
        void Awake()
        {
            baseStats = GameObject.FindWithTag(Config.playerTag).GetComponent<BaseStats>();
        }

        void Update()
        {
            GetComponent<Text>().text = baseStats.GetLevel().ToString();
        }
    }
}