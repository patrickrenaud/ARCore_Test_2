using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class AppFlowManager : MonoBehaviour
    {
        private static AppFlowManager _instance;
        public static float _timeInApp = 0;
        public AppFlowManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<AppFlowManager>();
                }
                return _instance;
            }
        }

        public string _startSceneName = "00_main";
        private static bool _isPaused = false;

        private void Awake()
        {
            if (_instance != null)
            {
                ResetApp();
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            _isPaused = false;
        }

        private void Update()
        {
            if (!_isPaused)
            {
                _timeInApp += Time.deltaTime;
            }
        }

        private void ResetApp()
        {
            WindowManager.Reset();
            EmailManager.Reset();
            _isPaused = false;
            _timeInApp = 0;
        }

        private void OnApplicationPause(bool pause)
        {
            _isPaused = pause;
            if (pause)
            {
                Debug.Log("App paused");
#if !UNITY_EDITOR
                FadeToScene.Instance.ChangeScene(_startSceneName);
#endif
            }
        }
    }
}
