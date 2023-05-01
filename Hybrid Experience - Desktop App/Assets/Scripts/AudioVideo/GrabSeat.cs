using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.AudioVideo
{
    public class GrabSeat : MonoBehaviour
    {
        [SerializeField] private AudioClip grabSound;

        public void PlayGrabSound()
        {
            VRUtils.Instance.PlaySpatialClipAt(grabSound, transform.position, 0.5f);
        }
    }
}

