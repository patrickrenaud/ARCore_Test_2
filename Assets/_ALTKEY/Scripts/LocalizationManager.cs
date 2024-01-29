// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ca.altkey
{
    public class LocalizationManager : MonoBehaviour
    {
        public delegate void OnLanguageChange();
        public static event OnLanguageChange _onLanguageChanged;

        public AudioClip _clickFX;

        public static LocalizationManager _instance;
        public static LangType _lang = LangType.NONE;

        private AudioSource _audioSource;

        public enum LangType
        {
            EN,
            FR,
            NONE
        }

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (_lang == LangType.NONE)
                {
                    _lang = LangType.EN;
                }
                SceneManager.sceneLoaded += OnSceneLoaded;
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            UpdateLocalization();
        }

        public void Start()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            UpdateLocalization();
        }

        public void UpdateLocalization()
        {
            LocalizationUpdate[] localizationItem = Resources.FindObjectsOfTypeAll<LocalizationUpdate>();
            for (int i = 0; i < localizationItem.Length; i++)
            {
                LocalizationUpdate item = localizationItem[i];
                item.gameObject.SetActive(item._lang == _lang);
            }
            if (_onLanguageChanged != null)
            {
                _onLanguageChanged();
            }
        }

        public void SetLanguage(string lang = "fr")
        {
            LangType requestLang = GetLangTypeByString(lang);
            if (requestLang != _lang)
            {
                _lang = requestLang;
                UpdateLocalization();
            }
        }

        public void ToggleLanguage()
        {
            _audioSource.clip = _clickFX;
            _audioSource.Play();

            _lang = _lang == LangType.FR ? LangType.EN : LangType.FR;
            UpdateLocalization();
        }

        private LangType GetLangTypeByString(string lang)
        {
            switch (lang)
            {
                case "en":
                    return LangType.EN;
                case "fr":
                default:
                    return LangType.FR;
            }
        }
    }
}
