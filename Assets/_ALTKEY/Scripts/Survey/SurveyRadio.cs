using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class SurveyRadio : MonoBehaviour
    {
        public GameObject[] _optionsSelected;
        public GameObject[] _optionsDefault;
        public int _selectedOption = -1;

        public void SelectOption(int id)
        {
            _selectedOption = id;
            for(int i = 0; i < _optionsDefault.Length; i++)
            {
                if (i == id)
                {
                    _optionsSelected[i].SetActive(true);
                    _optionsDefault[i].SetActive(false);
                }
                else
                {
                    _optionsSelected[i].SetActive(false);
                    _optionsDefault[i].SetActive(true);
                }
            }
        }


    }
}
