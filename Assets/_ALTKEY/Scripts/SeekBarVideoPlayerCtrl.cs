// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ca.altkey
{
    [RequireComponent(typeof(Slider))]
	public class SeekBarVideoPlayerCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public VideoPlayer _videoPlayer;
        public ManageVideoPlayer _manager;
        private Slider _slider;
        private bool _doUpdate = true;
        private bool _isPaused = false;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.value = 0f;
            _slider = GetComponent<Slider>();
            _isPaused = _videoPlayer.isPaused;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _doUpdate = false;
            _isPaused = _videoPlayer.isPaused;
            _videoPlayer.Pause();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _doUpdate = true;
            //if (!_isPaused)
            {
                _isPaused = false;
                _videoPlayer.Play();
                _manager.UnPause();
            }
            OnChangeValue();
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnChangeValue();
        }

        public void OnChangeValue()
        {
            _videoPlayer.time = _slider.value * _videoPlayer.length;
        }

        // Update is called once per frame
        void Update () 
		{
            if(_doUpdate && !_isPaused)
            {
                if((float)_videoPlayer.length == 0f)
                {
                    _slider.value = 0f;
                }
                else
                {
                    _slider.value = Mathf.Clamp01((float)_videoPlayer.time / (float)_videoPlayer.length);
                }
            }
        }
    }
}