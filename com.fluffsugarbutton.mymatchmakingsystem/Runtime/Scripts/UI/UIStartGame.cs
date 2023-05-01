using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] Button _startGame; 
    [SerializeField] TMP_InputField username; 
    // Start is called before the first frame update
    void Start()
    {
        _startGame.onClick.AddListener(StartGame);
    }

    private void StartGame(){
        MainManager.Instance.username = username.text;
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyOptions);
    }
}
