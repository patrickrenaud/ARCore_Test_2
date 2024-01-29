// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ca.altkey
{
    public class AdjustVolumeCtrl : MonoBehaviour
    {
        private static float _mainVolume = 1f;

        private void Awake()
        {
            AudioListener.volume = _mainVolume;
        }

        public void OnVolumeSliderChange(Slider slider)
        {
            _mainVolume = slider.normalizedValue;
            AudioListener.volume = _mainVolume;
        }

    }
}