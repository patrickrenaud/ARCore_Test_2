// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    [RequireComponent(typeof(Animator))]
    public class VoiceOverMarkerSync : MonoBehaviour
    {
        private string START_ANIM_FLAG = "startAnim";

        public AudioClip _en;
        public AudioClip _fr;

        public bool _controlAudioSouce = true;
        public bool _controlAnim = true;
        public AudioSource _audioSource;
        public Renderer _renderer;

        private Animator _animator;

        private string _enAnim = "english";
        private string _frAnim = "french";
        private string _idleAnim = "idle";
        private string _currentAnim;

        private bool _isEnabled = false;

        void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_renderer == null)
            {
                _renderer = GetComponentInChildren<Renderer>();
            }
            if(_controlAudioSouce)
            {
                if(_audioSource == null)
                { 
                    _audioSource = GetComponent<AudioSource>();
                }
                _audioSource.Stop();
            }
            UpdateSourceLanguage();
        }

        private void OnEnable()
        {
            LocalizationManager._onLanguageChanged += UpdateSourceLanguage;
        }

        private void OnDisable()
        {
            LocalizationManager._onLanguageChanged -= UpdateSourceLanguage;
        }

        private void OnDestroy()
        {
            LocalizationManager._onLanguageChanged -= UpdateSourceLanguage;
        }

        private void UpdateSourceLanguage()
        {
            if(LocalizationManager._lang == LocalizationManager.LangType.EN)
            {
                if(_controlAudioSouce)
                {
                    _audioSource.clip = _en;
                }
                if(_controlAnim)
                {
                    _currentAnim = _enAnim;
                }
            }
            else
            {
                if (_controlAudioSouce)
                {
                    _audioSource.clip = _fr;
                }
                if (_controlAnim)
                {
                    _currentAnim = _frAnim;
                }
            }
            if (_controlAudioSouce)
            {
                _audioSource.time = 0;
            }
        }

        void Update()
        {
            if(_renderer != null)
            {
                if (!_isEnabled && _renderer.enabled)
                {
                    _isEnabled = true;
                    UpdateAudioTime();
                }
                else if (_isEnabled && !_renderer.enabled)
                {
                    _isEnabled = false;
                    _animator.Play(_idleAnim);
                    if (_controlAudioSouce)
                    {
                        _audioSource.Pause();
                    }
                }
            }
        }

        private void UpdateAudioTime()
        {
            if (_controlAudioSouce)
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.UnPause();
                }
                else
                {
                    _audioSource.Play();
                }
            }
            if (_controlAnim)
            {
                _animator.Play(_currentAnim, 0, _audioSource.time / _audioSource.clip.length);
            }
        }

    }
}