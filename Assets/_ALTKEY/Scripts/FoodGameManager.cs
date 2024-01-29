// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class FoodGameManager : MonoBehaviour
    {
        public ChangePage _changePage;
        public List<GameObject> _foodItemsMsg = new List<GameObject>();

        public AudioClip _congratsFX;
        public AudioClip _wrongFX;
        public AudioClip _endFX;
        public GameObject _congratulation;
        public GameObject _noCongratulationBkg;
        public GameObject _congratulationBkg;

        private AudioSource _audioSource;
        private List<DragAndDropFoodItem> _foodItemsPlaced = new List<DragAndDropFoodItem>();
        private bool _gameCompleted = false;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            DragAndDropFoodItem._onFoodItemPlaced += OnFoodItemPlaced;
        }

        private void OnDisable()
        {
            DragAndDropFoodItem._onFoodItemPlaced -= OnFoodItemPlaced;
        }

        private void OnDestroy()
        {
            DragAndDropFoodItem._onFoodItemPlaced -= OnFoodItemPlaced;
        }

        private void OnFoodItemPlaced(DragAndDropFoodItem foodItem)
        {
            if(!_foodItemsPlaced.Contains(foodItem))
            {
                if(foodItem._goInCupboard)
                {
                    _audioSource.clip = _congratsFX;
                }
                else
                {
                    _audioSource.clip = _wrongFX;
                }
                _audioSource.Play();
                _foodItemsPlaced.Add(foodItem);
                foodItem.gameObject.SetActive(foodItem._goInCupboard);
                _congratulationBkg.SetActive(foodItem._goInCupboard);
                _noCongratulationBkg.SetActive(!foodItem._goInCupboard);

                CloseAllMsg();
                _foodItemsMsg[foodItem._foodItemIndex].SetActive(true);
                _congratulation.SetActive(true);
                CheckGameCompletion();
            }
        }

        private void CloseAllMsg()
        {
            for(int i = 0; i < _foodItemsMsg.Count; i++)
            {
                _foodItemsMsg[i].SetActive(false);
            }
        }

        private void CheckGameCompletion()
        {
            if(_foodItemsPlaced.Count == _foodItemsMsg.Count)
            {
                _gameCompleted = true;
            }
        }

        public void TryToShowGameCompletion()
        {
            //called by close button of the message popup
            if(_gameCompleted)
            {
                _audioSource.clip = _endFX;
                _audioSource.Play();
                _changePage.NextPage();
            }
        }
    }
}