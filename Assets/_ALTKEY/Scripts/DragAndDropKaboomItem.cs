// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ca.altkey
{
    public class DragAndDropKaboomItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void OnKaboomItemPlaced(DragAndDropKaboomItem kaboomItem);
        public static event OnKaboomItemPlaced _onKaboomItemPlaced;

        public int _kaboomItemIndex = -1;
        public bool _goInPowderRoom = true;
        public float _transitionTime = 0.25f;
        public BoxCollider2D _powderRoomBoxCollider2D;
        public CircleCollider2D _powderRoomCircleCollider2D;
        private bool _drag = false;
        [SerializeField] private float _dragSpeed = .5f;

        private Vector3 _initialPosition;
        void Awake()
        {
            _kaboomItemIndex--;
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

            TestPowderRoomCollision();
        }

        private void TestPowderRoomCollision()
        {
            if (_powderRoomBoxCollider2D.OverlapPoint(transform.position) || _powderRoomCircleCollider2D.OverlapPoint(transform.position))
            {
                _initialPosition = transform.position;
                if (_onKaboomItemPlaced != null)
                {
                    _onKaboomItemPlaced(this);
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
            if (_drag)
            {
                transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y)*_dragSpeed;
            }
        }

    }
}