using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ca.altkey
{
    public class EmailValidator : MonoBehaviour
    {
        public bool _activated = false;
        private Button _buttonToEnable;
        public TextMeshProUGUI _textToChange;

        private void Start()
        {
            _buttonToEnable = GetComponent<Button>();
        }

        public void ValidateEmail()
        {
            _activated = true;
            ValidateEmail(_textToChange.text);
        }

        public void SetEmailValid()
        {
            _activated = false;
            _buttonToEnable.interactable = true;
            _textToChange.color = Color.white;
        }

        public void ValidateEmail(string s)
        {
            if (!_activated) return;
            bool isValid = IsEmailValid(s);
            _buttonToEnable.interactable = isValid;
            if (isValid)
            {
                _textToChange.color = Color.white;
            }
            else
            {
                _textToChange.color = Color.red;
            }
        }

        public bool IsEmailValid(string s)
        {
            return RegexUtilities.IsValidEmail(s);
        }
    }
}
