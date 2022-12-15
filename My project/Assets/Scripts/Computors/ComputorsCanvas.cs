using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

namespace DIMuseumVR.Computors
{
    public class ComputorsCanvas : MonoBehaviour
    {
        [SerializeField]
        private List<ComputorSO> computors = new List<ComputorSO>();
        private int numberOfComputors = 0;
        private int indexOfComputors = 0;

        [SerializeField]
        private VideoPlayer videoPlayer;

        [SerializeField]
        private GameObject videoCanvasImage;
        private bool isVideoOn = false;

        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private List<BNG.Button> buttons = new List<BNG.Button>();

        private Computor computor;

        // Start is called before the first frame update
        void Start()
        {
            numberOfComputors = computors.Count;
            indexOfComputors = 0;
            ShowComputorContents();

            isVideoOn = false;
            videoCanvasImage.SetActive(isVideoOn);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                PreviousComputorButton();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                NextComputorButton();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                ToggleVideoButton();
            }
        }

        private void ShowComputorContents()
        {
            computor = computors[indexOfComputors].GetComputor();

            if (computor != null)
            {
                image.sprite = computor.Image;
                title.text = computor.Title;
                text.text = computor.Text;
            }
        }

        public void PreviousComputorButton()
        {
            indexOfComputors--;
            if (indexOfComputors < 0)
                indexOfComputors = numberOfComputors - 1;

            ShowComputorContents();
        }

        public void NextComputorButton()
        {
            indexOfComputors++;
            if (indexOfComputors > numberOfComputors - 1)
                indexOfComputors = 0;

            ShowComputorContents();
        }

        public void ToggleVideoButton()
        {
            isVideoOn = !isVideoOn;

            if (isVideoOn)
            {
                videoCanvasImage.SetActive(true);
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Stop();
                videoCanvasImage.SetActive(false);
            }
        }

        public void TogglePhysicalButtons(bool canButtonsBePressed)
        {
            Debug.LogWarning(canButtonsBePressed);
            foreach (var button in buttons)
            {
                button.enabled = canButtonsBePressed;
            }
        }
    }
}
