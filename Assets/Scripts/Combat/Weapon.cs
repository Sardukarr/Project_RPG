using UnityEngine;
using UnityEditor;
using RPG.Core;
namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        public void OnHit()
        {
            var soundFX = GetComponentInChildren<SFXRandomizer>();
            if(soundFX!=null)
                soundFX.RandomizeAndPlay();
        }
    }
}