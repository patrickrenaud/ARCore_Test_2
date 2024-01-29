// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _camera;
        private Transform _transform;
        void Start()
        {
            _transform = transform;
            _camera = Camera.main.transform;
        }
    
        
        void Update()
        {
            _transform.LookAt(_camera);
        }
    }
}