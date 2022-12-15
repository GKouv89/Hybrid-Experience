using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.SceneManagement.NotUsed
{
    // This Scriptable Object will serve just as a location to save the spawn point. This will persist among different scenes.
    // (Instead of using a persistent Singleton Manager with spawning data)
    [CreateAssetMenu(fileName = "SpawningSO", menuName = "ScriptableObjects/SpawningSO", order = 3)]
    public class SpawningSO : ScriptableObject
    {
        [SerializeField]
        private SerializedSpawnPoint spawnPoint;
        public SerializedSpawnPoint SpawnPoint
        {
            get { return spawnPoint; }
            set { spawnPoint = value; }
        }
    }
}
