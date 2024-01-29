// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToSceneLinker : MonoBehaviour
{

    public void ChangeScene(string sceneName)
    {
        FadeToScene.Instance.ChangeScene(sceneName);
    }

}
