using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DIMuseumVR.AudioVideo
{
    public class WelcomeVideo : MonoBehaviour
    {
        [SerializeField] float startDelay = 2f;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.targetTexture.Release();

            yield return new WaitForSeconds(startDelay);

            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }
    }
}
