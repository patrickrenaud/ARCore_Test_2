// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ca.altkey
{
    public class KeyboardManager : MonoBehaviour
    {
        public TextMeshProUGUI _numberTxt;
        public TextMeshProUGUI _msgTxt_FR;
        public TextMeshProUGUI _msgTxt_EN;

        public float _disabledAlphaColor = 0.4f;
        public int _maxNumber = 32;

        public string _msgTooLowNumber_EN;
        public string _msgTooLowNumber_FR;

        public string _msgTooHighNumber_EN;
        public string _msgTooHighNumber_FR;


        public AudioClip _clickFX;

        public Button _resetAppButton;
        public Button _confirmBtn;
        public Button _clearBtn;
        public Image _confirmIcon;
        public WindowManager _windowManager;

        private AudioSource _audioSource;

        private string _currentNumber = "";
        private int _requestedNumber = -1;

        private string _specialResetNumber = "0";
        private Color _defaultIconColor;
        private Color _disableIconColor;

        void Start()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            _defaultIconColor = new Color(_confirmIcon.color.r, _confirmIcon.color.g, _confirmIcon.color.b);
            _disableIconColor = new Color(_confirmIcon.color.r, _confirmIcon.color.g, _confirmIcon.color.b, _disabledAlphaColor);

            DisableConfirmButton();

            _clearBtn.gameObject.SetActive(false);
            _msgTxt_FR.gameObject.SetActive(false);
            _msgTxt_EN.gameObject.SetActive(false);
            _confirmIcon.gameObject.SetActive(true);
        }

        private void DisableConfirmButton()
        {
            _msgTxt_FR.gameObject.SetActive(true);
            _msgTxt_EN.gameObject.SetActive(true);

            _confirmBtn.interactable = false;

            _confirmIcon.color = _disableIconColor;
            _confirmIcon.gameObject.SetActive(false);

        }

        private void EnableConfirmButton()
        {
            _confirmBtn.interactable = true;
            _confirmIcon.gameObject.SetActive(true);
            _confirmIcon.color = _defaultIconColor;
        }

        public void ClearNumberEntry()
        {
            _audioSource.clip = _clickFX;
            _audioSource.Play();

            _requestedNumber = -1;
            _currentNumber = "";
            CheckNumberValidity();

            DisableConfirmButton();

            _clearBtn.gameObject.SetActive(false);
            _msgTxt_FR.gameObject.SetActive(false);
            _msgTxt_EN.gameObject.SetActive(false);
            _confirmIcon.gameObject.SetActive(true);
        }

        public void EnterNumber(int nbr)
        {
            _audioSource.clip = _clickFX;
            _audioSource.Play();

            _currentNumber = _numberTxt.text + nbr.ToString();
            if(_currentNumber.Length > 2)
            {
                _currentNumber = _currentNumber.Substring(1, 2);
            }

            _resetAppButton.gameObject.SetActive(_currentNumber == _specialResetNumber);

            CheckNumberValidity();
        }

        public void TriggerListButtonItem()
        {
            if(_requestedNumber < 0)
            {
                return;
            }

            _windowManager.TriggerListButtonItem(_requestedNumber);
        }

        public bool CheckNumberValidity()
        {
            int parsedResult;

            if (int.TryParse(_currentNumber, out parsedResult))
            {
                _clearBtn.gameObject.SetActive(true);
                _numberTxt.text = parsedResult.ToString();
                if(parsedResult <= 0)
                {
                    DisableConfirmButton();
                    _msgTxt_FR.text = _msgTooLowNumber_FR;
                    _msgTxt_EN.text = _msgTooLowNumber_EN;
                    return false;
                }
                else if (parsedResult > _maxNumber)
                {
                    DisableConfirmButton();
                    _msgTxt_FR.text = _msgTooHighNumber_FR;
                    _msgTxt_EN.text = _msgTooHighNumber_EN;
                    return false;
                }
                else
                {
                    _requestedNumber = parsedResult;
                    _msgTxt_FR.gameObject.SetActive(false);
                    _msgTxt_EN.gameObject.SetActive(false);
                    EnableConfirmButton();
                    return true;
                }
            }
            else
            {
                _requestedNumber = -1;
                _numberTxt.text = "";
                DisableConfirmButton();
                _msgTxt_FR.gameObject.SetActive(false);
                _msgTxt_EN.gameObject.SetActive(false);
                _clearBtn.gameObject.SetActive(false);
                _confirmIcon.color = _disableIconColor;
                return false;
            }
        }
    }
}