using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.SceneManagement.NotUsed
{
    public class SpawnPointVisual : MonoBehaviour
    {
        [SerializeField]
        private Color gizmoColor = Color.cyan;
        private void OnDrawGizmosSelected()
        {
            float spawnHeight = 0.7f;
            float spawnSize = spawnHeight / 2f;
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(this.transform.position , this.transform.position + this.transform.forward);
            Gizmos.DrawWireCube(this.transform.position - (this.transform.up * spawnSize), new Vector3(spawnSize, spawnHeight, spawnSize));
        }
    }
}
