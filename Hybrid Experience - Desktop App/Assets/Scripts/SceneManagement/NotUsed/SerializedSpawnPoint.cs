using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.SceneManagement.NotUsed
{
    [System.Serializable]
    public class SerializedSpawnPoint
    {
        [SerializeField]
        private SVector3 position;

        [SerializeField]
        private SQuaternion rotation;

        public SerializedSpawnPoint(SVector3 pos, SQuaternion rot)
        {
            this.position = pos;
            this.rotation = rot;
        }

        public SVector3 GetPosition()
        {
            return position;
        }

        public SQuaternion GetRotation()
        {
            return rotation;
        }

        public void SetPosRot(SVector3 pos, SQuaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}
