// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class ChangeLevel : MonoBehaviour
    {
        public AudioClip _clickFX;

        public GameObject _clickBlocker;
        public GameObject _buttonUp;
        public GameObject _buttonDown;
        public List<RectTransform> _levels;

        private AudioSource _audioSource;
        private float _transitionTime = .75f;
        private float _ratioOut = 0.5f;
        public static int _levelIndex = 1;
        private float _centerY;
        private float _screenHeight = -1f;

        void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            if (_levels.Count >= 1)
            {
                _centerY = _levels[0].position.y;
            }
            GameObject level;
            for (int i = 0; i < _levels.Count; i++)
            {
                level = _levels[i].gameObject;
                level.SetActive(i == _levelIndex);
            }
        }

        public void LevelDown()
        {
            UpdateLevel(-1);
        }

        public void LevelUp()
        {
            UpdateLevel(1);
        }

        private void UpdateLevel(int dir)
        {
            _audioSource.clip = _clickFX;
            _audioSource.Play();

            _clickBlocker.SetActive(true);
            RectTransform previousLevel = _levels[_levelIndex];
            _levelIndex += dir;

            if (_levelIndex <= 0)
            {
                _levelIndex = 0;
                _buttonUp.SetActive(true);
                _buttonDown.SetActive(false);
            }
            else if(_levelIndex >= _levels.Count - 1)
            {
                _levelIndex = _levels.Count - 1;
                _buttonUp.SetActive(false);
                _buttonDown.SetActive(true);
            }
            else
            {
                _buttonUp.SetActive(true);
                _buttonDown.SetActive(true);
            }

            RectTransform nextLevel = _levels[_levelIndex];

            Vector3[] fourCornersArray = InitializeArray<Vector3>(4);
            previousLevel.GetWorldCorners(fourCornersArray);
            float posY0 = fourCornersArray[0].y;
            float posY2 = fourCornersArray[2].y;
            _screenHeight = posY2 - posY0;

            float moveDistance;
            if (dir < 0)
            {
                moveDistance = -_screenHeight * dir;
            }
            else
            {
                moveDistance = -_screenHeight * _ratioOut * dir;
            }

            Vector3 pos = previousLevel.position + new Vector3(0f, moveDistance, 0f);
            iTween.MoveTo(previousLevel.gameObject, iTween.Hash("position", pos, "easeType", "easeOutCubic", "time", _transitionTime));

            nextLevel.gameObject.SetActive(true);
            if (dir < 0)
            {
                nextLevel.position = new Vector3(nextLevel.position.x, _centerY + (_screenHeight * _ratioOut * dir), nextLevel.position.z);
                moveDistance = _screenHeight * _ratioOut * dir;
            }
            else
            {
                nextLevel.position = new Vector3(nextLevel.position.x, _centerY + (_screenHeight * dir), nextLevel.position.z);
                moveDistance = _screenHeight * dir;
            }
            pos = nextLevel.position + new Vector3(0f, -moveDistance, 0f);
            iTween.MoveTo(nextLevel.gameObject, iTween.Hash("position", pos, "easeType", "easeOutCubic", "time", _transitionTime, "oncomplete", "OnCompleteChangeLevel", "oncompletetarget", gameObject, "oncompleteparams", previousLevel));
        }

        T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }

        public void OnCompleteChangeLevel(RectTransform previousLevel)
        {
            //previousLevel.gameObject.SetActive(false);
            _clickBlocker.SetActive(false);
        }
    }
}