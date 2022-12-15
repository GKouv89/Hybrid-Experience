using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.Utils
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private Vector3 axis;


        // Update is called once per frame
        void Update()
        {
            transform.Rotate(axis, speed * Time.deltaTime);
        }
    }
}
