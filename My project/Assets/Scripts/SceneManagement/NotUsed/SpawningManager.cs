using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;

namespace DIMuseumVR.SceneManagement.NotUsed
{
    public class SpawningManager : MonoBehaviour
    {
        public static SpawningManager Instance = null;

        [SerializeField]
        private SpawningSO spawningSOEntrance, spawningSOSection1, spawningSOSection2, spawningSOSection3, spawningSOSection4, spawningSOSection5, spawningSOSection6;

        private SpawningSO currentSpawningSO;
        private SerializedSpawnPoint currentSpawningPoint;

        private PlayerTeleport playerTeleport;


        private void Awake()
        {
            // !!! This is not a correct singleton because it doesn't destroy other existing instances in the scene (for example when reloading the same scene)
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);

                GameObject player = GameObject.FindWithTag("Player");
                playerTeleport = player.GetComponent<PlayerTeleport>();
                currentSpawningSO = spawningSOSection6;
            }
            else
            {
                Destroy(this);
            }

            TeleportToSpawnPoint();
        }

        private void TeleportToSpawnPoint()
        {
            currentSpawningPoint = currentSpawningSO.SpawnPoint;
            if (currentSpawningPoint == null)
            {
                Debug.LogError("The Spawning Scriptable Object doesn't have a point assigned.");
            }
            else
            {
                bool isPlayerTeleportEnabled = playerTeleport.enabled;
                if (!isPlayerTeleportEnabled)
                {
                    playerTeleport.enabled = true;
                }

                playerTeleport.TeleportPlayer(currentSpawningPoint.GetPosition(), currentSpawningPoint.GetRotation());

                if (!isPlayerTeleportEnabled)
                {
                    playerTeleport.enabled = false;
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentSpawningSO = spawningSOSection1;
                SceneManager.LoadScene("SampleScene");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentSpawningSO = spawningSOSection1;
                SceneManager.LoadScene("SampleScene2");
             }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentSpawningSO = spawningSOSection1;
                SceneManager.LoadScene("SampleScene3");
            }
        }
    }
}
