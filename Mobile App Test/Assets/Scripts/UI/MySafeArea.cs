using UnityEngine;

public class MySafeArea : MonoBehaviour
{
    RectTransform rectTransform;
    Rect safeArea;
    Vector2 minAnchor;
    Vector2 maxAnchor;
    void Awake(){
        rectTransform = GetComponent<RectTransform>(); // Take RectTransform of Component
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;
        Debug.Log("Safe area position: " + safeArea.position);
        Debug.Log("Safe area size: " + safeArea.size);
        Debug.Log("minAnchor: " + minAnchor);
        Debug.Log("maxAnchor: " + maxAnchor);

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;
        Debug.Log("minAnchor.x: " + minAnchor.x);
        Debug.Log("minAnchor.y: " + minAnchor.y);
        Debug.Log("maxAnchor.x: " + maxAnchor.x);
        Debug.Log("maxAnchor.y: " + maxAnchor.y);

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }

}
