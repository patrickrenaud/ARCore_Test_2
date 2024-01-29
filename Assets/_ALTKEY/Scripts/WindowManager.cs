// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ca.altkey
{
    public class WindowManager : MonoBehaviour
    {
        public enum WindowMode
        {
            MAP,
            LIST,
            CAMERA,
            VIDEO,
            GAME_KABOOM,
            GAME_TOWER,
            GAME_FOOD,
            GAME_CANNON,
            KEYBOARD,
            SURVEY
        }

        public Scrollbar _scrollBarList;
        public ScrollRect _scrollRect;
        public GameObject _blocker;

        public AudioClip _clickFX;

        public List<Button> _listTriggerItems;
        public List<GameObject> _mapItems = new List<GameObject>();
        public List<GameObject> _mapButtonHighlight = new List<GameObject>();
        public List<GameObject> _listButtonVisited = new List<GameObject>();
        public List<GameObject> _listItems = new List<GameObject>();
        public List<GameObject> _cameraItems = new List<GameObject>();
        public List<GameObject> _keyboardItems = new List<GameObject>();
        public List<GameObject> _videoItems = new List<GameObject>();
        
        public bool _initPlaySound = false;

        public static List<bool> _visitedItem;
        private AudioSource _audioSource;
        public static WindowMode _currentMode = WindowMode.MAP;
        public static WindowMode _previousMode = WindowMode.MAP;
        public static float _listScrollPosition = 1f;
        private string _arScene = "02_ar";
        private string _mainScene = "00_main";

        public void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            
            GoToMode(_previousMode);
            JumpToListScrollPosition();
        }

        public void Start()
        {
            RefreshVisitedFlag();
        }
        

        public void RefreshVisitedFlag()
        {
            if (_visitedItem == null || _visitedItem.Count == 0)
            {
                _visitedItem = new List<bool>();
                
                for (int i = 0; i < _mapButtonHighlight.Count; i++)
                {
                    _visitedItem.Add(false);
                }
            }
            for(int i = 0; i < _mapButtonHighlight.Count; i++)
            {
                GameObject go = _mapButtonHighlight[i];
                go.SetActive(_visitedItem[i]);
            }
            for(int i = 0; i < _listButtonVisited.Count; i++)
            {
                GameObject go = _listButtonVisited[i];
                go.SetActive(_visitedItem[i]);
            }
        }

        public void UpdateVisitedItemFlag(int index, bool refresh = true)
        {
            _visitedItem[index] = true;
            if (refresh)
            {
                RefreshVisitedFlag();
            }
        }

        public void UpdateVisitedItemFlag(int index)
        {
            _visitedItem[index] = true;
            RefreshVisitedFlag();
        }

        public static void Reset()
        {
             _currentMode = WindowMode.MAP;
            _previousMode = WindowMode.MAP;
            _listScrollPosition = 1f;
        }

        private void JumpToListScrollPosition()
        {
            _scrollBarList.value = _listScrollPosition;
            _scrollRect.verticalNormalizedPosition = _listScrollPosition;
        }

        public void OnListScrollPositionChange(Scrollbar scrollBar)
        {
            _listScrollPosition = scrollBar.value;
        }

        public void ResetGame()
        {
            Reset();
            _visitedItem = null;
            _blocker.SetActive(true);
            FadeToScene.Instance.ChangeScene(_mainScene);
        }

        public void TriggerListButtonItem(int listNumber)
        {
            int index = listNumber - 1;
            if(index >= 0 || index < _listTriggerItems.Count)
            {
                _listTriggerItems[index].onClick.Invoke();
                _visitedItem[index] = true;
                RefreshVisitedFlag();
            }
        }

        private void PlayClickSound()
        {
            if(_initPlaySound)
            {
                _audioSource.clip = _clickFX;
                _audioSource.Play();
            }
            _initPlaySound = true;
        }

        public void ActivateMap()
        {
            PlayClickSound();

            UpdateActive(_listItems, false);
            UpdateActive(_cameraItems, false);
            UpdateActive(_videoItems, false);
            UpdateActive(_keyboardItems, false);
            UpdateActive(_mapItems, true);
            _previousMode = _currentMode;
            _currentMode = WindowMode.MAP;
        }

        public void ActivateList()
        {
            PlayClickSound();
            UpdateActive(_mapItems, false);
            UpdateActive(_cameraItems, false);
            UpdateActive(_videoItems, false);
            UpdateActive(_keyboardItems, false);
            UpdateActive(_listItems, true);
            _previousMode = _currentMode;
            _currentMode = WindowMode.LIST;
        }

        public void ActivateCamera(bool takePicture = false)
        {
            PlayClickSound();
            //UpdateActive(_mapItems, false);
            //UpdateActive(_listItems, false);
            //UpdateActive(_cameraItems, true);
            //UpdateActive(_videoItems, false);
            EmailManager._pictureEnabled = takePicture;
            _previousMode = _currentMode;
            _currentMode = WindowMode.CAMERA;
            _blocker.SetActive(true);
            FadeToScene.Instance.ChangeScene(_arScene);
        }

        public void ActivateHorseCamera()
        {
            PlayClickSound();
            EmailManager._pictureEnabled = true;
            _previousMode = _currentMode;
            _currentMode = WindowMode.CAMERA;
            _blocker.SetActive(true);
			BackToSceneLinker._isTakingHorsePicture = true;
			FadeToScene.Instance.ChangeScene("08_horsePicture");
        }

        public void ActivateKeyboard()
        {
            PlayClickSound();
            UpdateActive(_mapItems, false);
            UpdateActive(_cameraItems, false);
            UpdateActive(_videoItems, false);
            UpdateActive(_listItems, false);
            UpdateActive(_keyboardItems, true);
            _previousMode = _currentMode;
            _currentMode = WindowMode.KEYBOARD;
        }

        public void ActivateVideo()
        {
            PlayClickSound();
            UpdateActive(_mapItems, false);
            UpdateActive(_listItems, false);
            UpdateActive(_cameraItems, false);
            UpdateActive(_keyboardItems, false);
            UpdateActive(_videoItems, true);
            _previousMode = _currentMode;
            _currentMode = WindowMode.VIDEO;
        }

        public void ActivateGameTower()
        {
            PlayClickSound();
            _previousMode = _currentMode;
            _currentMode = WindowMode.GAME_TOWER;
            _blocker.SetActive(true);
            FadeToScene.Instance.ChangeScene("03_gameTower");
        }

        public void ActivateGameFood()
        {
            PlayClickSound();
            _previousMode = _currentMode;
            _currentMode = WindowMode.GAME_FOOD;
            _blocker.SetActive(true);
            FadeToScene.Instance.ChangeScene("04_gameFood");
        }

        public void ActivateGameCannon()
        {
            PlayClickSound();
            _previousMode = _currentMode;
            _currentMode = WindowMode.GAME_CANNON;
            _blocker.SetActive(true);
            FadeToScene.Instance.ChangeScene("05_gameCannon");
        }

        public void ActivateGameKaboom()
        {
            PlayClickSound();
            _previousMode = _currentMode;
            _currentMode = WindowMode.GAME_KABOOM;
            _blocker.SetActive(true);
            FadeToScene.Instance.ChangeScene("06_gameKaboom");
        }

        public void ActivateSurvey()
        {
            PlayClickSound();
            _previousMode = _currentMode;
            _currentMode = WindowMode.SURVEY;
            _blocker.SetActive(true);
            FadeToScene.Instance.ChangeScene("07_survey");
        }

        public void ReturnToPreviousMode()
        {
            if(_currentMode == _previousMode)
            {
                return;
            }

            GoToMode(_previousMode);
        }

        public void GoToMode(WindowMode mode)
        {
            switch (mode)
            {
                case WindowMode.MAP:
                    ActivateMap();
                    break;
                case WindowMode.LIST:
                    ActivateList();
                    break;
                case WindowMode.CAMERA:
                    ActivateCamera();
                    break;
                case WindowMode.VIDEO:
                    ActivateVideo();
                    break;
                case WindowMode.GAME_KABOOM:
                    ActivateGameKaboom();
                    break;
                case WindowMode.GAME_TOWER:
                    ActivateGameTower();
                    break;
                case WindowMode.GAME_FOOD:
                    ActivateGameFood();
                    break;
                case WindowMode.GAME_CANNON:
                    ActivateGameCannon();
                    break;
                case WindowMode.KEYBOARD:
                    ActivateKeyboard();
                    break;
                case WindowMode.SURVEY:
                    ActivateSurvey();
                    break;
            }
        }


        public void UpdateActive(List<GameObject> list, bool isActive)
        {
            foreach (GameObject go in list)
            {
                go.SetActive(isActive);
            }
        }

        public void DeactivateAll()
        {
            UpdateActive(_mapItems, false);
            UpdateActive(_listItems, false);
            UpdateActive(_cameraItems, false);
            UpdateActive(_videoItems, false);
        }
    }
}
