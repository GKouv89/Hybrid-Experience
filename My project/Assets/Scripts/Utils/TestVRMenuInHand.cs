using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIMuseumVR.Utils
{
    public class TestVRMenuInHand : MonoBehaviour
    {
        // TAKEN FROM DemoScript.cs

        /// <summary>
        /// GameObject used to show Debug Info in-game
        /// </summary>
        public GameObject DebugMenu;

        private int frames = 0;

        // Start is called before the first frame update
        void Start()
        {
            VRUtils.Instance.Log("From any script: output text here by using VRUtils.Log(\"Message Here\");");
            VRUtils.Instance.Log("Click the Menu button to toggle this menu.");
        }

        // Update is called once per frame
        void Update()
        {
            // Toggle Debug Menu by pressing menu button down => Better to use the ToggleActiveOnInputAction script
            //if (InputBridge.Instance && InputBridge.Instance.BackButtonDown)
            //{
            //DebugMenu.SetActive(!DebugMenu.activeSelf);
            //}

            frames++;
            //VRUtils.Instance.Log("frame number: " + frames.ToString());
        }
    }

}
