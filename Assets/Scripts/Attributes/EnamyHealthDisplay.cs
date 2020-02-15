using UnityEngine;
using RPG.Core;
using UnityEngine.UI;
using System;
using RPG.Combat;

namespace RPG.Attributes
{
    public class EnamyHealthDisplay : MonoBehaviour
    {
        Health health=null;
        Fighter playerFighter = null;
        void Awake()
        {
            playerFighter = GameObject.FindWithTag(Config.playerTag).GetComponent<Fighter>();
            
        }

        void Update()
        {
            health = playerFighter.GetTarget();
            if (health != null)
            {
                var values = health.GetCurrentAndMaxHealth();
                //    GetComponent<Text>().text = String.Format("{0:0}%",health.GetPercent());
                GetComponent<Text>().text = String.Format("{0:0}/{1}", values.Item1, values.Item2);
            }
            else
                GetComponent<Text>().text = "No target";
        }
    }
}