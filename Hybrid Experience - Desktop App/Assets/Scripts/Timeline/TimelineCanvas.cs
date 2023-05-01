using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

namespace DIMuseumVR.Timeline
{
    public class TimelineCanvas : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer videoPlayer;

        [SerializeField]
        private GameObject videoCanvasImage;

        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI text;

        private TimelinePeriod timelinePeriod;

        private void Start()
        {
            videoCanvasImage.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            TimelineTrigger timelineTrigger = other.GetComponent<TimelineTrigger>();

            if (timelineTrigger != null)
            {
                timelinePeriod = timelineTrigger.GetTimelinePeriod();
                if (timelinePeriod != null)
                {
                    if (timelinePeriod.VideoClip != null)
                    {
                        videoCanvasImage.gameObject.SetActive(true);
                        videoPlayer.clip = timelinePeriod.VideoClip;
                        videoPlayer.Play();
                    }
                    else
                    {
                        videoPlayer.Stop();
                        videoCanvasImage.gameObject.SetActive(false);
                        image.sprite = timelinePeriod.Image;
                        title.text = timelinePeriod.Title;
                        text.text = timelinePeriod.Text;
                    }
                }
            }
        }
    }
}
