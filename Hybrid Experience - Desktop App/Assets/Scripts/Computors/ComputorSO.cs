using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.Computors
{
    [CreateAssetMenu(fileName = "ComputorSO", menuName = "ScriptableObjects/ComputorSO", order = 2)]
    public class ComputorSO : ScriptableObject
    {
        [SerializeField]
        private Computor computor;

        public Computor GetComputor()
        {
            return computor;
        }
    }
}
