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
    public class ChangeAnimatorLocalizationFlag : MonoBehaviour
    {
        private string USE_ENGLISH_FLAG = "useEnglish";
        private Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            UpdateAnimatorLanguage();
        }

        private void OnEnable()
        {
            LocalizationManager._onLanguageChanged += UpdateAnimatorLanguage;
        }

        private void OnDisable()
        {
            LocalizationManager._onLanguageChanged -= UpdateAnimatorLanguage;
        }

        private void OnDestroy()
        {
            LocalizationManager._onLanguageChanged -= UpdateAnimatorLanguage;
        }

        private void UpdateAnimatorLanguage()
        {
            _animator.SetBool(USE_ENGLISH_FLAG, LocalizationManager._lang == LocalizationManager.LangType.EN);
        }
    }
}