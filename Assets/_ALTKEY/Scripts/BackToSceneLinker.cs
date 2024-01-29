using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class BackToSceneLinker : MonoBehaviour
    {
        public static bool _isTakingHorsePicture = false;

        public void BackToPreviousScene()
        {
            if (_isTakingHorsePicture)
            {
                _isTakingHorsePicture = false;
                FadeToScene.Instance.ChangeScene("07_survey");
            }
            else
            {
                FadeToScene.Instance.ChangeScene("01_visit");
            }
        }
    }
}