// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ca.altkey
{
    [RequireComponent(typeof(Image))]
    public class ScaleQuadToImageSize : MonoBehaviour 
	{
        public Transform _quad;
        public VideoPlayer _videoPlayer;
        public bool _adaptToAspectRatio = true;
        private Image _imageRef;
        private Canvas _canvasRef;
        // Use this for initialization
        void Start () 
		{
            _imageRef = GetComponent<Image>();
            _canvasRef = GetComponentInParent<Canvas>();
            Rect size = RectTransformUtility.PixelAdjustRect(_imageRef.rectTransform, _canvasRef);
            _quad.localScale = new Vector3(size.width, size.height, 1f);
            if(_adaptToAspectRatio && _videoPlayer != null)
            {
                StartCoroutine(UpdateVideoSize(size.width, size.height));
            }
        }

        private IEnumerator UpdateVideoSize(float wMax, float hMax)
        {
            yield return new WaitUntil(() => _videoPlayer.texture != null);
            float h = _videoPlayer.texture.height;
            float w = _videoPlayer.texture.width;

            float h2 = (h * wMax)/ w;
            float w2 = (w * hMax) / h;

            if(h2 <= hMax)
            {
                _quad.localScale = new Vector3(wMax, h2, 1f);
            }
            else if(w2 <= wMax)
            {
                _quad.localScale = new Vector3(w2, hMax, 1f);
            }
        }
    }
}