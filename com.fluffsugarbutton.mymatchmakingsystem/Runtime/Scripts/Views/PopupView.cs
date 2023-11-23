using UnityEngine;
using UnityEngine.UIElements;


namespace MatchMaking.LobbySetup.UI
{
    public class PopupView : MonoBehaviour
    {
        private UIDocument myDocRef;
        void OnDisable()
        {
            Debug.Log("DISABLED");
        }
        void OnEnable()
        {
            Debug.Log("ENABLED");
        }

        void Awake()
        {
            // The UXML is already instantiated by the UIDocument component
            myDocRef = GetComponent<UIDocument>();
        }

        public void ShowPopup()
        {
            // this.enabled = true;
            myDocRef.rootVisualElement.style.display = DisplayStyle.Flex;
            Button okButton = myDocRef.rootVisualElement.Q<Button>();
            okButton.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            okButton.clicked += () => {
                Debug.Log("Clickity");
                MainManager.Instance.LeaveLobby();
            };
        }

        public void HidePopup()
        {
            myDocRef.rootVisualElement.style.display = DisplayStyle.None;
            // this.enabled = false;
        }
    }
}
