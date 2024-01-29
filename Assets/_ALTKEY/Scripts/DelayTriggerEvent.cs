// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayTriggerEvent : MonoBehaviour
{
    public float _delay = 0f;
    public UnityEvent _event;
    
    void Start()
    {
        if(_event != null)
        {
            StartCoroutine(TriggerEvent());
        }
    }

    private IEnumerator TriggerEvent()
    {
        yield return new WaitForSeconds(_delay);
        _event.Invoke();
    }
}
