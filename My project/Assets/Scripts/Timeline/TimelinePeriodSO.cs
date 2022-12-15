using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.Timeline
{
    [CreateAssetMenu(fileName = "TimelinePeriodSO", menuName = "ScriptableObjects/TimelinePeriodSO", order = 1)]
    public class TimelinePeriodSO : ScriptableObject
    {
        [SerializeField]
        private TimelinePeriod timelinePeriod;

        public TimelinePeriod GetTimelinePeriod()
        {
            return timelinePeriod;
        }
    }
}
