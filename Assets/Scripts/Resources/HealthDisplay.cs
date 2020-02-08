using UnityEngine;
using RPG.Core;
using UnityEngine.UI;
using System;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health=null;
        void Awake()
        {
            health = GameObject.FindWithTag(Config.playerTag).GetComponent<Health>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}%",health.GetPercent());
        }
    }
}