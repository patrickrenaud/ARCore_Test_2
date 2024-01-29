// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ca.altkey
{
    public class DragAndDropFoodItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void OnFoodItemPlaced(DragAndDropFoodItem foodItem);
        public static event OnFoodItemPlaced _onFoodItemPlaced;

        public int _foodItemIndex = -1;
        public bool _goInCupboard = true;
        public float _transitionTime = 0.25f;
        public Collider2D _cupboard;
        private bool _drag = false;
        [SerializeField] private float _dragSpeed = .5f;
        private Vector3 _initialPosition;
        void Awake()
        {
            _foodItemIndex--;
            _initialPosition = transform.position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _drag = true;
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.SetAsLastSibling();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _drag = false;

            TestCupboardCollision();
        }

        private void TestCupboardCollision()
        {
            if (_cupboard.bounds.Contains(transform.position))
            {
                _initialPosition = transform.position;
                if(_onFoodItemPlaced != null)
                {
                    _onFoodItemPlaced(this);
                }
            }
            else
            {
                // return to initial position
                iTween.MoveTo(gameObject, iTween.Hash("position", _initialPosition, "easeType", "easeOutCubic", "time", _transitionTime));
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_drag)
            {
                transform.localPosition += new Vector3(eventData.delta.x , eventData.delta.y)*_dragSpeed;
            }
        }

    }
}