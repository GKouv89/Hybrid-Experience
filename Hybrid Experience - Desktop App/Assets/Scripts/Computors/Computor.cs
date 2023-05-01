using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.Computors
{
    [System.Serializable]
    public class Computor
    {
        [SerializeField]
        private Sprite image;
        public Sprite Image { get { return image; } }

        [SerializeField]
        private string title;
        public string Title { get { return title; } }

        [SerializeField]
        [TextArea(3, 10)]
        private string text;
        public string Text { get { return text; } }
    }
}
