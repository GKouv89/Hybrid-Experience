using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIMuseumVR.Timeline
{
    public class TimelineTrigger : MonoBehaviour
    {
        [SerializeField]
        private TimelinePeriodSO timelinePeriodSO;

        public TimelinePeriod GetTimelinePeriod()
        {
            if (timelinePeriodSO != null)
                return timelinePeriodSO.GetTimelinePeriod();
            else
                return null;
        }
    }
}
