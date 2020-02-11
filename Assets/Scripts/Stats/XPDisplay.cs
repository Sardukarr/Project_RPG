using UnityEngine;
using RPG.Core;
using UnityEngine.UI;
using System;

namespace RPG.Stats
{
    public class XPDisplay : MonoBehaviour
    {
        Experience experience = null;
        void Awake()
        {
            experience = GameObject.FindWithTag(Config.playerTag).GetComponent<Experience>();
        }

        void Update()
        {
            GetComponent<Text>().text = experience.GetXP().ToString();
        }
    }
}