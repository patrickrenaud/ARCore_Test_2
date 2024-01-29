using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class VoiceOverMarker : MonoBehaviour
    {

        public AudioClip _en;
        public AudioClip _fr;

        public bool _controlAudioSouce = true;
        public AudioSource _audioSource;
        public Renderer _renderer;

        private bool _isEnabled = false;

        // Start is called before the first frame update
        void Start()
        {

            if (_renderer == null)
            {
                _renderer = GetComponentInChildren<Renderer>();
            }
            if (_controlAudioSouce)
            {
                if (_audioSource == null)
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
            if (LocalizationManager._lang == LocalizationManager.LangType.EN)
            {
                if (_controlAudioSouce)
                {
                    _audioSource.clip = _en;
                }
            }
            else
            {
                if (_controlAudioSouce)
                {
                    _audioSource.clip = _fr;
                }
            }
            if (_controlAudioSouce)
            {
                _audioSource.time = 0;
            }
        }

        void Update()
        {
            if (_renderer != null)
            {
                if (!_isEnabled && _renderer.enabled)
                {
                    _isEnabled = true;
                    UpdateAudioTime();
                }
                else if (_isEnabled && !_renderer.enabled)
                {
                    _isEnabled = false;
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
        }

    }
}