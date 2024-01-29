using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class GeneralInfoManager : MonoBehaviour
{
    public TextMeshProUGUI _batteryLifeInfoTxt;
    public TextMeshProUGUI _timeInfoTxt;

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.batteryLevel >= 0)
        {
            _batteryLifeInfoTxt.SetText(Mathf.CeilToInt(SystemInfo.batteryLevel * 100f).ToString().Trim() + "%");
        }
        _timeInfoTxt.SetText(DateTime.Now.ToString("h:mm"));
    }
}