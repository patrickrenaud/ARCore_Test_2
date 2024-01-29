// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace ca.altkey
{
    [RequireComponent(typeof(VideoPlayer))]
	public class VideoPlayerURLLinker : MonoBehaviour
	{
        public string _videoName;
        private string _videoPath = "Videos";
        private void Awake()
        {
            VideoPlayer vid = GetComponent<VideoPlayer>();
            vid.source = VideoSource.Url;
            vid.aspectRatio = VideoAspectRatio.NoScaling;
            vid.url = Path.Combine(Application.streamingAssetsPath, _videoPath, _videoName);
        }
    }
}