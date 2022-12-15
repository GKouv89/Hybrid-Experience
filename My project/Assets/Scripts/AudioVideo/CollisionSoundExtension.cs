using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.AudioVideo
{
    public class CollisionSoundExtension : MonoBehaviour
    {
        public AudioClip CollisionAudio;
        public float resetLastPlayedDuration = 0.1f;
        public float minCollisionVelocity = 0.1f;
        AudioSource audioSource;
        float startTime;

        Collider col;

        Grabbable grab;

        /// <summary>
        /// Volume will never be played below this amount. 0-1
        /// </summary>
        public float MinimumVolume = 0.25f;

        /// <summary>
        /// Cap volume at this level 0 - 1
        /// </summary>
        public float MaximumVolume = 1f;

        public bool RecentlyPlayedSound = false;

        float lastPlayedSound;
        public float LastRelativeVelocity = 0;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1f;
            }

            startTime = Time.time;
            col = GetComponent<Collider>();
            grab = GetComponent<Grabbable>();
        }

        private void OnCollisionEnter(Collision collision)
        {

            // Just spawned, don't fire collision sound immediately
            if (Time.time - startTime < 0.2f)
            {
                return;
            }

            // No Collider present, don't play sound
            if (col == null || !col.enabled)
            {
                return;
            }

            float colVelocity = collision.relativeVelocity.magnitude;

            bool otherColliderPlayedSound = false;
            var colSound = collision.collider.GetComponent<CollisionSoundExtension>();
            // Don't play a sound if something else is playing the same sound.
            // This prevents overlap
            if (colSound)
            {
                otherColliderPlayedSound = colSound.RecentlyPlayedSound && colSound.CollisionAudio == CollisionAudio;
            }

            float soundVolume = Mathf.Clamp(collision.relativeVelocity.magnitude / 10, MinimumVolume, MaximumVolume);
            bool minVelReached = colVelocity > minCollisionVelocity;

            bool audioValid = audioSource != null && CollisionAudio != null;

            if (minVelReached && audioValid && !otherColliderPlayedSound)
            {
                // If object is being held play the sound very lightly
                if (grab != null && grab.BeingHeld)
                {
                    minVelReached = true;
                    soundVolume = 0.05f;
                }


                LastRelativeVelocity = colVelocity;

                // Play Shot
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                audioSource.clip = CollisionAudio;
                audioSource.pitch = Time.timeScale;
                audioSource.volume = soundVolume;
                audioSource.Play();

                RecentlyPlayedSound = true;

                Invoke("resetLastPlayedSound", resetLastPlayedDuration);
            }
        }

        void resetLastPlayedSound()
        {
            RecentlyPlayedSound = false;
        }
    }
}
