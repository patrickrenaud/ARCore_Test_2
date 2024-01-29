// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace ca.altkey
{
    public class CannonGameManager : MonoBehaviour
    {
        public TextMeshProUGUI _stepTxt;
        public TextMeshProUGUI _stepTitleTxt;

        public float _timeBeforeDisplayEndMsg = 5f;

        public Color _brownColor;
        public Color _highlightColor;

        public AudioClip _wrongFX;
        public AudioClip _rightFX;
        public AudioClip _explosionFX;
        public AudioClip _endFX;

        public GameObject _endMsg;
        private GameObject _currentExplanation;
        private GameObject _currentCircle;
        private GameObject _currentHighlight;

        public int _cannonStep;
        public List<GameObject> _steps;

        public List<GameObject> _errorSteps;
        public List<GameObject> _okSteps;
        public List<GameObject> _highlightCircles;

        private AudioSource _audioSource;
        private int _nextStep = 1;
        private string _bColor;
        private string _hColor;
        private bool _gameIsDone = false;

        void Start()
        {
            if(_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            _bColor = ColorUtility.ToHtmlStringRGB(_brownColor);
            _hColor = ColorUtility.ToHtmlStringRGB(_highlightColor);
            UpdateStepProgress(0);
        }

        public void Awake()
        {
            LocalizationManager._onLanguageChanged += OnLanguageChange;
        }

        public void OnDisable()
        {
            LocalizationManager._onLanguageChanged -= OnLanguageChange;
        }

        public void OnDestroy()
        {
            LocalizationManager._onLanguageChanged -= OnLanguageChange;
        }

        private void OnLanguageChange()
        {
            UpdateStepProgress(_nextStep);
        }

        public void SelectItem(GameObject item)
        {
            if (_endMsg.activeSelf)
            {
                return;
            }
            CannonItem cannonItem = item.GetComponent<CannonItem>();
            int itemOrder = cannonItem._orderInList;
            int itemIndex = itemOrder - 1;
            UpdateStep(itemOrder);
            DeactivatePreviousItemMsg();

            _currentCircle = _highlightCircles[itemIndex];
            _currentCircle.SetActive(true);
            CircleLinker circleLinker = _currentCircle.GetComponent<CircleLinker>();
            circleLinker._okCircle.SetActive(itemOrder == _nextStep);
            circleLinker._errorCircle.SetActive(itemOrder != _nextStep);

            _currentHighlight = cannonItem._highlightBkg;
            _currentHighlight.SetActive(true);

            if (itemOrder == _nextStep)
            {
                if(itemOrder == _cannonStep)
                {
                    _audioSource.clip = _explosionFX;
                }
                else
                {
                    _audioSource.clip = _rightFX;
                }
                _audioSource.Play();

                //this is the right choice
                _currentExplanation = _okSteps[itemIndex];
                cannonItem._numberIcon.SetActive(true);
                item.transform.SetSiblingIndex(itemIndex);
                UpdateStepProgress(_nextStep);
                _nextStep++;
                if (_nextStep > _steps.Count)
                {
                    if(!_gameIsDone)
                    {
                        _gameIsDone = true;
                        //the game is over
                        StartCoroutine(ShowEndMessage());
                    }
                }
            }
            else
            {
                _audioSource.clip = _wrongFX;
                _audioSource.Play();

                _currentExplanation = _errorSteps[itemIndex];
            }
            _currentExplanation.SetActive(true);
        }

        private void DeactivatePreviousItemMsg()
        {
            if (_currentExplanation != null)
            {
                _currentExplanation.SetActive(false);
            }
            if (_currentCircle != null)
            {
                _currentCircle.SetActive(false);
            }
            if (_currentHighlight != null)
            {
                _currentHighlight.SetActive(false);
            }
        }

        IEnumerator ShowEndMessage()
        {
            yield return new WaitForSeconds(_timeBeforeDisplayEndMsg);
            DeactivatePreviousItemMsg();
            _stepTitleTxt.gameObject.SetActive(false);
            _stepTxt.gameObject.SetActive(false);
            _endMsg.SetActive(true);

            _audioSource.clip = _endFX;
            _audioSource.Play();
        }

        public void UpdateStepProgress(int step)
        {
            string label = LocalizationManager._lang == LocalizationManager.LangType.EN ? "Step" : "Étape";
            string link = LocalizationManager._lang == LocalizationManager.LangType.EN ? "of" : "de";
            _stepTxt.text = "<color=#" + _bColor + ">" + label + " </color><color=#" + _hColor + ">" + step + "</color> " + link + "<color=#" + _bColor + "> " + _steps.Count + "</color>";
        }

        public void UpdateStep(int step)
        {
            _stepTitleTxt.gameObject.SetActive(true);
            string label = LocalizationManager._lang == LocalizationManager.LangType.EN ? "step" : "étape";
            _stepTitleTxt.text = "<color=#" + _hColor + ">" + label + " " + step + "</color>";
        }

    }
}