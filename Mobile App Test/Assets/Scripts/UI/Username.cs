using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Username : MonoBehaviour
{
    private TMP_InputField username;
    public Button startGame;

    void Awake(){
        username = this.gameObject.GetComponent<TMP_InputField>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Adds a listener to the main input field and invokes a method when the value changes.
        username.onValueChanged.AddListener(ValueChangeCheck);        
    }

    // Invoked when the value of the text field changes.
    public void ValueChangeCheck(string text)
    {
        if(string.IsNullOrEmpty(text)){
            startGame.interactable = false;
            return;
        }
        startGame.interactable = true;
    }
}
