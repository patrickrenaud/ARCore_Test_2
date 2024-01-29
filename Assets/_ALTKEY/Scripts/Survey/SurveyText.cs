using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class SurveyText : MonoBehaviour
    {
        public GameObject _labels;
        public GameObject _inputField;

        public string _input = "";

        public void SetInput(string input)
        {
            _input = input;
            if (string.IsNullOrWhiteSpace(_input))
            {
                if (_labels) _labels.SetActive(true);
            }
            else
            {
                if (_labels) _labels.SetActive(false);
            }
        }
    }
}
