// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float _speed = -200;
    public AXIS _aroundAxis = AXIS.Z;
    public enum AXIS
    {
        X,
        Y,
        Z
    }
    
    void Update()
    {
        if(_speed != 0f)
        {
            Vector3 rotation;
            switch(_aroundAxis)
            {
                case AXIS.X:
                    rotation = new Vector3(_speed * Time.deltaTime, 0f,0f);
                    break;
                case AXIS.Y:
                    rotation = new Vector3(0f, _speed * Time.deltaTime, 0f);
                    break;
                case AXIS.Z:
                default:
                    rotation = new Vector3(0f, 0f, _speed * Time.deltaTime);
                    break;
            }
            transform.Rotate(rotation);
        }
    }
}
