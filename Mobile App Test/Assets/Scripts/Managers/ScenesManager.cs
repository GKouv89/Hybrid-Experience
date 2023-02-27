using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// From Dani Krossing's CHANGE SCENE WITH BUTTON IN UNITY
// https://www.youtube.com/watch?v=jrPTpD2eAMw

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public enum Scene
    {
        EnterUsernameScene,
        CreateJoinRoomScene,
        SampleScene
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadCreateRoom()
    {
        SceneManager.LoadScene(Scene.CreateJoinRoomScene.ToString());
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadEnterUsername()
    {
        SceneManager.LoadScene(Scene.EnterUsernameScene.ToString());
    }
}
