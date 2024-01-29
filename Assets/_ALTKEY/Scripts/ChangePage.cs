// <copyright file=LookAt company="Studio ALTKEY inc.">
// Copyright © All Rights Reserved
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePage : MonoBehaviour
{
    public string _nextScene = "01_visit";
    public AudioClip _clip;
    public AudioSource _audioSource;
    public GameObject _clickBlocker;
    public List<RectTransform> _pages;

    private float _transitionTime = .75f;
    private float _ratioOut = 1f/2f;
    private int _pageIndex = 0;
    private float _centerX;
    private float _screenWidth = -1f;

    void Awake()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        if (_pages.Count >= 1)
        {
            RectTransform rectP = _pages[0];
            _centerX = rectP.position.x;
        }

        for (int i = 1; i < _pages.Count; i++)
        {
            GameObject page = _pages[i].gameObject;
            page.SetActive(false);
        }
        
    }

    public void NextPage()
    {
        if (_audioSource != null)
        {
            _audioSource.clip = _clip;
            _audioSource.Play();
        }
        NextPageNoSound();
    }

    public void NextPageNoSound()
    {
        _clickBlocker.SetActive(true);
        RectTransform previousPage = _pages[_pageIndex];
        _pageIndex++;

        if (_pageIndex >= _pages.Count)
        {
            _pageIndex = _pages.Count - 1;
            ChangeScene();
            return;
        }
        RectTransform nextPage = _pages[_pageIndex];
        nextPage.SetAsLastSibling();

        Vector3[] fourCornersArray = InitializeArray<Vector3>(4);
        previousPage.GetWorldCorners(fourCornersArray);
        float posX0 = fourCornersArray[0].x;
        float posX2 = fourCornersArray[2].x;
        _screenWidth = posX2 - posX0;

        float moveDistance = _screenWidth * _ratioOut;
        Vector3 pos = previousPage.position + new Vector3(-moveDistance, 0f, 0f);
        iTween.MoveTo(previousPage.gameObject, iTween.Hash("position", pos, "easeType", "easeOutCubic", "time", _transitionTime));

        nextPage.gameObject.SetActive(true);
        nextPage.position = new Vector3(nextPage.position.x + _screenWidth, nextPage.position.y, nextPage.position.z);
        iTween.MoveTo(nextPage.gameObject, iTween.Hash("x", 0, "easeType", "easeOutCubic", "time", _transitionTime, "oncomplete", "OnCompleteChangePage", "oncompletetarget", gameObject, "oncompleteparams", previousPage));
    }

    T[] InitializeArray<T>(int length) where T : new()
    {
        T[] array = new T[length];
        for (int i = 0; i < length; ++i)
        {
            array[i] = new T();
        }

        return array;
    }

    public void ChangeScene()
    {
        if (_audioSource != null)
        {
            _audioSource.clip = _clip;
            _audioSource.Play();
        }

        FadeToScene.Instance.ChangeScene(_nextScene);
    }

    public void OnCompleteChangePage(RectTransform previousPage)
    {
        previousPage.gameObject.SetActive(false);
        _clickBlocker.SetActive(false);
    }
}
