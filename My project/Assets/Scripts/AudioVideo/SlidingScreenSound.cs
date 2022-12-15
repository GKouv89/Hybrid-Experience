using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.AudioVideo
{
    public class SlidingScreenSound : MonoBehaviour
    {
        [SerializeField] private AudioClip sliderSound;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void OnSliderUpdate(float drawerValue)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(sliderSound);
            }
        }
    }
}
