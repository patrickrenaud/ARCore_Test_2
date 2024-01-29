using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maxstAR;
using System;

public class MarkerContentLoader : MarkerTrackerBehaviour
{
	public TrackerManager.TrackingOption _trackingOption;
	private PrefabLoader _prefabLoader;

	void Awake()
	{
		_prefabLoader = GetComponentInChildren<PrefabLoader>();
    }

    public override void OnTrackSuccess(string id, string name, Matrix4x4 poseMatrix)
    {
        base.OnTrackSuccess(id, name, poseMatrix);
		int idNum;
		if(Int32.TryParse(id, out idNum))
		{
			TrackerManager.GetInstance().SetTrackingOption(_trackingOption);
			_prefabLoader.LoadPrefab(idNum);
		}
    }
}
