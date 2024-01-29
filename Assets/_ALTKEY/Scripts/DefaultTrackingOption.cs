// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using maxstAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class DefaultTrackingOption : MonoBehaviour
    {
        public TrackerManager.TrackingOption _trackingOption;
        
        void Start()
        {
            TrackerManager.GetInstance().SetTrackingOption(_trackingOption);
        }
    }
}