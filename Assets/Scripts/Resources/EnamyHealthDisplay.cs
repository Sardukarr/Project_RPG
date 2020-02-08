using UnityEngine;
using RPG.Core;
using UnityEngine.UI;
using System;
using RPG.Combat;

namespace RPG.Resources
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
                GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercent());
            else
                GetComponent<Text>().text = "No target";
        }
    }
}