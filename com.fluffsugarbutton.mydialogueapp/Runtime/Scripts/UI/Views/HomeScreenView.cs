using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueApp
{
    public class HomeScreenView : MonoBehaviour
    {
        UIDocument uIDocument;
        [SerializeField]
        List<Conversation> chats;
        [SerializeField]
        VisualTreeAsset chatPreviewTemplate;
        [SerializeField]
        List<GameObject> panels; // the panels correspond one to one to the chats.
        
        void OnEnable()
        {
            uIDocument = GetComponent<UIDocument>();
            // This part is so that the click input from the first person controller's inputsystem will
            // actually manage to go through, and reach the texture on which the ui document is getting rendered.
            // Exact copy paste from: https://www.youtube.com/watch?v=gXx_j-6z8jY
            // This only applies for the Desktop app, as the mobile app has no additional controls.
            #if UNITY_STANDALONE_WIN
                uIDocument.panelSettings.SetScreenToPanelSpaceFunction((Vector2 screenPosition) => {
                    var invalidPosition = new Vector2(float.NaN, float.NaN);
                    var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Debug.DrawRay(cameraRay.origin, cameraRay.direction * 100, Color.magenta);
                    
                    RaycastHit hit;
                    if(!Physics.Raycast(cameraRay, out hit, Mathf.Infinity, LayerMask.GetMask("UI")))
                    {
                        return invalidPosition;
                    }else{
                    }

                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.y = 1 - pixelUV.y;
                    pixelUV.x *= uIDocument.panelSettings.targetTexture.width;
                    pixelUV.y *= uIDocument.panelSettings.targetTexture.height;

                    return pixelUV;
                });
            #endif
            HomeScreenController controller = new();
            controller.Initialize(uIDocument.rootVisualElement, chats, panels, chatPreviewTemplate);
            controller.FillHomeScreen();
        }
    }
}