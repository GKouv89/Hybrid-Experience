using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] Button _startGame; 
    // Start is called before the first frame update
    void Start()
    {
        _startGame.onClick.AddListener(StartGame);
    }

    private void StartGame(){
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyOptions);
    }
}
