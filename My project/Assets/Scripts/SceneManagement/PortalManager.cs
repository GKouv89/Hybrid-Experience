using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;

namespace DIMuseumVR.SceneManagement
{
    public class PortalManager : MonoBehaviour
    {
        [SerializeField]
        private bool testKeys = true;

        [SerializeField]
        private Transform playerXRRig;

        private const string key = "lastPortalLocation";

        void Start()
        {
            // Load last portal location from PlayerPrefs and teleport the player to that portal
            string lastPortalLocationString = PlayerPrefs.GetString(key);
            System.Enum.TryParse(lastPortalLocationString, out PortalLocation lastPortalLocation);
            Portal lastPortal = FindLastPortal(lastPortalLocation);
            playerXRRig.SetPositionAndRotation(lastPortal.transform.position, lastPortal.transform.rotation);

            StartCoroutine(ResetStartingLocation());
        }

        IEnumerator ResetStartingLocation()
        {
            yield return new WaitForSeconds(1f);
            SaveLastPortalLocationInPlayerPrefs(PortalLocation.Entrance);   // So the app will start in the Entrance the next time it runs 
        }

        private Portal FindLastPortal(PortalLocation lastLocation)
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            Portal lastPortal = portals.FirstOrDefault(t => t.PortalLocation == lastLocation);
            return lastPortal;
        }

        private void SaveLastPortalLocationInPlayerPrefs(PortalLocation portalLocation)
        {
            string lastPortalLocationString = portalLocation.ToString();
            PlayerPrefs.SetString(key, lastPortalLocationString);
            PlayerPrefs.Save();
        }

        private void Update()
        {
            if (Application.isEditor && testKeys)
            {
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    OnSection4Button();
                }

                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    OnSection5Button();
                }

                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    OnSection6Button();
                }
            }
        }

        public void OnSection4Button()
        {
            SaveLastPortalLocationInPlayerPrefs(PortalLocation.Section4);
            SceneManager.LoadScene("MultiplayerBandu");
        }

        public void OnSection5Button()
        {
            SaveLastPortalLocationInPlayerPrefs(PortalLocation.Section5);
            SceneManager.LoadScene("Tron-Single");
        }

        public void OnSection6Button()
        {
            SaveLastPortalLocationInPlayerPrefs(PortalLocation.Section6);
            SceneManager.LoadScene("MixedReality");
        }

        // Doesn't work correctly in Android
        private void OnApplicationQuit()
        {
            SaveLastPortalLocationInPlayerPrefs(PortalLocation.Entrance);   // So the app will start in the Entrance the next time it runs 
        }

        private void OnApplicationPause(bool pause)
        {
            SaveLastPortalLocationInPlayerPrefs(PortalLocation.Entrance);   // So the app will start in the Entrance the next time it runs 
        }
    }
}