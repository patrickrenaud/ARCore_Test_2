// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ca.altkey
{
	public class ManageVideoPlayer : MonoBehaviour 
	{
        public VideoPlayer _videoPlayer;
        public Slider _controlSlider;
        public Button _playBtn;
        public Button _pauseBtn;

        public WindowManager _windowManager;

        public List<VideoClip> _videoClipsFR;
        public List<VideoClip> _videoClipsEN;

        private bool _videoIsReady = false;
        private AudioClip _clip;
        private SubtitleBlock _currentSubtitle;
        private bool _pause = false;

        private void OnEnable()
        {
            if(!_videoIsReady)
            {
                _videoPlayer.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
            }
            _videoPlayer.loopPointReached += OnVideoEnd;
            UnPauseNoSound();
        }

        private void OnDisable()
        {
            _videoPlayer.loopPointReached -= OnVideoEnd;
        }

        private void OnVideoEnd(VideoPlayer source)
        {
            source.Stop();
            _windowManager.ReturnToPreviousMode();
        }

        public void SetNextVideoClip(int index)
        {
            if(index >= _videoClipsFR.Count || index < 0)
            {
                return;
            }

            if(LocalizationManager._lang == LocalizationManager.LangType.FR)
            {
                _videoPlayer.clip = _videoClipsFR[index];
            }
            else
            {
                _videoPlayer.clip = _videoClipsEN[index];
            }
        }

        public void Pause()
        {
            PauseNoSound();
        }

        public void Update()
        {
            if(!_videoIsReady && _videoPlayer.isPlaying)
            {
                _videoPlayer.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
                _videoIsReady = true;
            }
        }

        public void PauseNoSound()
        {
            _pause = true;
            UpdatePause();
        }

        public void UnPause()
        {
            UnPauseNoSound();
        }

        public void UnPauseNoSound()
        {
            _pause = false;
            UpdatePause();
        }

        private void UpdatePause()
        {
            if (_pause)
            {
                _videoPlayer.Pause();
            }
            else
            {
                _videoPlayer.Play();
            }
            _playBtn.gameObject.SetActive(_pause);
            _pauseBtn.gameObject.SetActive(!_pause);
        }

    }
}