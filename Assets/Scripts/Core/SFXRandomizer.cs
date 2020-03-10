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
        AudioSource myAudioSource = null;

        private void Awake()
        {
            myAudioSource = GetComponent<AudioSource>();
        }
        private void Update()
        {
           // TimeSinceLastChange += Time.deltaTime;
        }
        public void RandomizeAndPlay()
        {
            if (myAudioSource!=null && !myAudioSource.isPlaying)
            {
               
                if(audioClips.Length >0)
                {
                    int value = UnityEngine.Random.Range(0, audioClips.Length - 1);

                    myAudioSource.clip = audioClips[value];
                    myAudioSource.pitch = UnityEngine.Random.Range(Range.Item1, Range.Item2);
                    myAudioSource.Play();
                }
            }
        }
    }
     
}