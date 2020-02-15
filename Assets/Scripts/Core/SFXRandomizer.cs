using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
 
    public class SFXRandomizer : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClips = null;
        [SerializeField] Tuple<float, float> Range = new Tuple<float, float>(0.9f,1.1f);
        float TimeSinceLastChange = 10f;
        float HowOffenChange = 2f;
        private void Update()
        {
           // TimeSinceLastChange += Time.deltaTime;
        }
        public void RandomizeAndPlay()
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                int value = UnityEngine.Random.Range(0, audioClips.Length - 1);
                var audioSource = GetComponent<AudioSource>();
                audioSource.clip = audioClips[value];
                audioSource.pitch = UnityEngine.Random.Range(Range.Item1, Range.Item2);
                audioSource.Play();
                
            }
        }
    }
     
}