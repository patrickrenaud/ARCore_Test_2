// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToScene : MonoBehaviour
{
    public static FadeToScene _instance = null;

    public Image _image;
    public GameObject _loadingScreen;
    public AudioClip _clip;
    private float _transitionTime = 1.5f;
    private AsyncOperation _asyncOperation;

    public static FadeToScene Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<FadeToScene>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_image == null)
        {
            _image = GetComponentInChildren<Image>(true);
        }
        _image.gameObject.SetActive(false);
    }

    public void UpdateAlpha(float alpha)
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
    }

    public void ChangeScene(string sceneName)
    {
        if (_instance != null && !_instance.Equals(this))
        {
            _instance.ChangeScene(sceneName);
            return;
        }

        //Begin to load the Scene you specify
        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //Don't let the Scene activate until you allow it to
        _asyncOperation.allowSceneActivation = false;

        _image.gameObject.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "easeType", "easeOutExpo", "time", _transitionTime, "onupdate", "UpdateAlpha", "oncomplete", "OnFadeInComplete", "oncompletetarget", gameObject, "oncompleteparams", sceneName));
    }

    protected void OnFadeInComplete(string sceneName)
    {
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        _asyncOperation.allowSceneActivation = true;
        _loadingScreen.SetActive(true);
        while (!_asyncOperation.isDone)
        {
            yield return new WaitForEndOfFrame();
            yield return null;
        }
        _loadingScreen.SetActive(false);
        iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "easeType", "easeOutExpo", "time", _transitionTime/2, "onupdate", "UpdateAlpha", "oncomplete", "OnFadeOutComplete", "oncompletetarget", gameObject));
    }

    protected void OnFadeOutComplete()
    {
        _image.gameObject.SetActive(false);
    }

}
