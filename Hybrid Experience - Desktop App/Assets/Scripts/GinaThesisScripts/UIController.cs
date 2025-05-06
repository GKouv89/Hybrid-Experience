using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject PipBoy;
    [SerializeField]
    Button OpenMenu;
    void Start()
    {
        OpenMenu.onClick.AddListener(() => {
            Debug.Log("Pip boy is: " + PipBoy.activeSelf);
            PipBoy.SetActive(!PipBoy.activeSelf);
        });
    }
}
