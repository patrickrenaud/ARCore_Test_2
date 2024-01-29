// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class KaboomGameManager : MonoBehaviour
    {
        public ChangePage _changePage;
        public List<GameObject> _kaboomItemsMsg = new List<GameObject>();

        public AudioClip _rightFX;
        public AudioClip _explosionFX;
        public AudioClip _endFX;

        public GameObject _congratulation;
        public GameObject _noCongratulationBkg;
        public GameObject _congratulationBkg;

        private AudioSource _audioSource;
        private List<DragAndDropKaboomItem> _kaboomItemsPlaced = new List<DragAndDropKaboomItem>();
        private bool _gameCompleted = false;

        void Start()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            DragAndDropKaboomItem._onKaboomItemPlaced += OnKaboomItemPlaced;
        }

        private void OnDisable()
        {
            DragAndDropKaboomItem._onKaboomItemPlaced -= OnKaboomItemPlaced;
        }

        private void OnDestroy()
        {
            DragAndDropKaboomItem._onKaboomItemPlaced -= OnKaboomItemPlaced;
        }

        private void OnKaboomItemPlaced(DragAndDropKaboomItem kaboomItem)
        {
            if (!_kaboomItemsPlaced.Contains(kaboomItem))
            {
                if (kaboomItem._goInPowderRoom)
                {
                    _audioSource.clip = _rightFX;
                }
                else
                {
                    _audioSource.clip = _explosionFX;
                }
                _audioSource.Play();

                _kaboomItemsPlaced.Add(kaboomItem);
                kaboomItem.gameObject.SetActive(kaboomItem._goInPowderRoom);
                _congratulationBkg.SetActive(kaboomItem._goInPowderRoom);
                _noCongratulationBkg.SetActive(!kaboomItem._goInPowderRoom);

                CloseAllMsg();
                _kaboomItemsMsg[kaboomItem._kaboomItemIndex].SetActive(true);
                _congratulation.SetActive(true);
                CheckGameCompletion();
            }
        }

        private void CloseAllMsg()
        {
            for (int i = 0; i < _kaboomItemsMsg.Count; i++)
            {
                _kaboomItemsMsg[i].SetActive(false);
            }
        }

        private void CheckGameCompletion()
        {
            if (_kaboomItemsPlaced.Count == _kaboomItemsMsg.Count)
            {
                _gameCompleted = true;
            }
        }

        public void TryToShowGameCompletion()
        {
            //called by close button of the message popup
            if (_gameCompleted)
            {
                _audioSource.clip = _endFX;
                _audioSource.Play();

                _changePage.NextPage();
            }
        }
    }
}