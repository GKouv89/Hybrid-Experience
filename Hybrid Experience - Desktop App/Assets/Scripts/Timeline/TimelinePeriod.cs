using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DIMuseumVR.Timeline
{
    [System.Serializable]
    public class TimelinePeriod
    {
        [SerializeField]
        private VideoClip videoClip;
        public VideoClip VideoClip { get { return videoClip; } }

        [SerializeField]
        private Sprite image;
        public Sprite Image { get { return image; } }

        [SerializeField]
        private string title;
        public string Title { get { return title; } }

        [SerializeField]
        [TextArea(3,10)]
        private string text;
        public string Text { get { return text; } }
    }
}
