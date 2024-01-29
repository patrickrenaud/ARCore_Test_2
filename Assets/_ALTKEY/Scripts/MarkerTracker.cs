// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using maxstAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class MarkerTracker : ARBehaviour
    {
        private Dictionary<int, MarkerTrackerBehaviour> markerTrackableMap =
            new Dictionary<int, MarkerTrackerBehaviour>();

        private CameraBackgroundBehaviour cameraBackgroundBehaviour = null;

        void Awake()
        {
            Init();

            cameraBackgroundBehaviour = FindObjectOfType<CameraBackgroundBehaviour>();
            if (cameraBackgroundBehaviour == null)
            {
                Debug.LogError("Can't find CameraBackgroundBehaviour.");
                return;
            }
        }

        void Start()
        {
            MarkerTrackerBehaviour[] markerTrackables = FindObjectsOfType<MarkerTrackerBehaviour>();

            foreach (var trackable in markerTrackables)
            {
                trackable.SetMarkerTrackerFileName(trackable.MarkerID, trackable.MarkerSize);
                markerTrackableMap.Add(trackable.MarkerID, trackable);
            }
            AddTrackerData();
            StartCamera();
            TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_MARKER);
        }

        private void AddTrackerData()
        {
            foreach (var trackable in markerTrackableMap)
            {
                if (trackable.Value.TrackerDataFileName.Length == 0)
                {
                    continue;
                }

                TrackerManager.GetInstance().AddTrackerData(trackable.Value.TrackerDataFileName);
            }

            TrackerManager.GetInstance().LoadTrackerData();
        }

        private void DisableAllTrackables()
        {
            foreach (var trackable in markerTrackableMap)
            {
                trackable.Value.OnTrackFail();
            }
        }

        void Update()
        {
            DisableAllTrackables();

            TrackingState state = TrackerManager.GetInstance().UpdateTrackingState();

            cameraBackgroundBehaviour.UpdateCameraBackgroundImage(state);
            TrackingResult trackingResult = state.GetTrackingResult();

            string recognizedID = null;
            for (int i = 0; i < trackingResult.GetCount(); i++)
            {
                Trackable trackable = trackingResult.GetTrackable(i);
                int markerId = -1;
                if (int.TryParse(trackable.GetName(), out markerId))
                {
                    if (markerTrackableMap.ContainsKey(markerId))
                    {
                        markerTrackableMap[markerId].OnTrackSuccess(
                            trackable.GetId(), trackable.GetName(), trackable.GetPose());

                        recognizedID += trackable.GetId().ToString() + ", ";
                    }
                }
            }
        }

        void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                TrackerManager.GetInstance().StopTracker();
                StopCamera();
            }
            else
            {
                StartCamera();
                TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_MARKER);
            }
        }

        void OnDestroy()
        {
            markerTrackableMap.Clear();
            TrackerManager.GetInstance().StopTracker();
            TrackerManager.GetInstance().DestroyTracker();
            StopCamera();
        }

        public void DisableTracking()
        {
            PrefabLoader.ClearPrefab();
            DisableAllTrackables();
            TrackerManager.GetInstance().StopTracker();
            StopCamera();
        }

        public void EnableTracking()
        {
            StartCamera();
            TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_MARKER);
        }
    }
}