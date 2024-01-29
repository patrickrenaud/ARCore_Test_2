using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAspectRatio : MonoBehaviour
{

    public Camera _camera;

    
    // Update is called once per frame
    void Update()
    {
        _camera.aspect = 1.333f;
    }
}
