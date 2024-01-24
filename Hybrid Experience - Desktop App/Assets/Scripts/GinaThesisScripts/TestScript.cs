using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestScript : MonoBehaviour
{
    Button myButton;
    // Start is called before the first frame update
    void Start()
    {
        myButton = this.gameObject.GetComponent<Button>();
        Debug.Log(myButton);
        myButton.onClick.AddListener(() => {
            Debug.Log("Clicky clicky");
        });
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
