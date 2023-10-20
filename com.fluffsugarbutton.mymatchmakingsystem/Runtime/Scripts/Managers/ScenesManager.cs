using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatchMaking
{
    public class ScenesManager : MonoBehaviour
    {
        public static ScenesManager Instance;

        private void Awake(){
            if(Instance != null){
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public enum Scene
        {
            UsernameScene,
            LobbyOptions,
            LobbyList,
            WaitingRoom,
            #if UNITY_ANDROID
                SampleScene,
            #elif UNITY_STANDALONE_WIN
                DIMuseumVR,
            #endif
        }

        public void LoadScene(Scene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        public void startGame()
        {
            ScenesManager.Scene gameScene; 
            #if UNITY_STANDALONE_WIN
                gameScene = ScenesManager.Scene.DIMuseumVR;
            #elif UNITY_ANDROID
                gameScene = ScenesManager.Scene.SampleScene;
            #endif
            ScenesManager.Instance.LoadScene(gameScene);
        }


        // public void LoadNextScene()
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // }
    }
}
