using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.Utils
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float time = 5f;

        void Start()
        {
            Destroy(this.gameObject, time);
        }
    }
}
