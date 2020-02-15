using UnityEngine;
using RPG.Core;
using UnityEngine.UI;
using System;

namespace RPG.Attributes
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
            var values = health.GetCurrentAndMaxHealth();
          //    GetComponent<Text>().text = String.Format("{0:0}%",health.GetPercent());
            GetComponent<Text>().text = String.Format("{0:0}/{1}", values.Item1,values.Item2);
        }
    }
}