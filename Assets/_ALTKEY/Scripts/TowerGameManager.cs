// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ca.altkey
{
    public class TowerGameManager : MonoBehaviour
    {
        public AudioSource _audioSource;
        public AudioClip _clickFX;
        public AudioClip _endFX;

        public Color _fontVisitedColor = Color.gray;
        private Color _fontSelectedColor = Color.black;
        public float _delayShowEndMessage = 3f;

        public GameObject _congratulationPanel;

        public List<GameObject> _buttons;
        public List<GameObject> _highlights;
        public List<GameObject> _infoBoxes;

        private bool _endMessageShown = false;

        private string HIGHLIGHT_TAG = "Highlight";

        private List<int> _visitedIndex = new List<int>();

        private int _lastIndexSelected = -1;

        public void SelectItem(int index)
        {
            index--;
            if (_lastIndexSelected >= 0)
            {
                GameObject previousButtonHighlight = GetHighlightInGameObject(_buttons[_lastIndexSelected]);
                if (previousButtonHighlight != null)
                {
                    previousButtonHighlight.SetActive(false);
                }
                _highlights[_lastIndexSelected].SetActive(false);
                _infoBoxes[_lastIndexSelected].SetActive(false);

                TextMeshProUGUI[] textMeshProUGUIs = _buttons[_lastIndexSelected].GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < textMeshProUGUIs.Length; i++)
                {
                    TextMeshProUGUI textMeshProUGUI = textMeshProUGUIs[i];
                    textMeshProUGUI.color = _fontVisitedColor;
                }
            }

            _audioSource.clip = _clickFX;
            _audioSource.Play();

            TextMeshProUGUI[] textMeshProUGUISelected = _buttons[index].GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < textMeshProUGUISelected.Length; i++)
            {
                TextMeshProUGUI textMeshProUGUI = textMeshProUGUISelected[i];
                textMeshProUGUI.color = _fontSelectedColor;
            }

            GameObject currentButtonHighlight = GetHighlightInGameObject(_buttons[index]);
            currentButtonHighlight.SetActive(true);
            _highlights[index].SetActive(true);
            _infoBoxes[index].SetActive(true);

            _lastIndexSelected = index;

            CheckEndOfGame(index);
        }

        private void CheckEndOfGame(int index)
        {
            if(!_visitedIndex.Contains(index))
            {
                _visitedIndex.Add(index);
            }

            if (_visitedIndex.Count >= _buttons.Count)
            {
                //trigger game end
                if(!_endMessageShown)
                {
                    _endMessageShown = true;
                    StartCoroutine(ShowEndMessage());
                }
            }
        }

        IEnumerator ShowEndMessage()
        {
            yield return new WaitForSeconds(_delayShowEndMessage);
            _audioSource.clip = _endFX;
            _audioSource.Play();
            _congratulationPanel.SetActive(true);
        }

        private GameObject GetHighlightInGameObject(GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.CompareTag(HIGHLIGHT_TAG))
                {
                    return child.gameObject;
                }
            }
            return null;
        }
    }
}