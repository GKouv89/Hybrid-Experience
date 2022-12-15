using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField]
        private PortalLocation portalLocation;
        public PortalLocation PortalLocation {  get => portalLocation; }

        [SerializeField]
        private Color gizmoColor = Color.cyan;
        private void OnDrawGizmosSelected()
        {
            float spawnHeight = 0.7f;
            float spawnSize = spawnHeight / 2f;
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward);
            Gizmos.DrawWireCube(this.transform.position - (this.transform.up * spawnSize), new Vector3(spawnSize, spawnHeight, spawnSize));
        }
    }

    public enum PortalLocation { Entrance, Section1, Section2, Section3, Section4, Section5, Section6 };
}
