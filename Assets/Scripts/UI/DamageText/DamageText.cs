using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        public void SetText(float dmg)
        {
            GetComponentInChildren<Text>().text = String.Format("{0:0}", dmg);
        }
    }

}