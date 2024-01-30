using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace ca.altkey
{
    [System.Serializable]
    public class SurveyAnswers
    {
        public string _date;
        public int _timeInApp;
        public int _langType;

        public int _accessibility;
        public int _welcome;
        public int _activity;
        public int _price;
        public int _language;
        public bool[] _heardAbout;
        public string _heardAboutOther;
        public int _age;
        public string _residence;
        public int _newsletter;
        public string _name;
        public string _email;
    }
    public class SurveyManager : MonoBehaviour
    {
        public string _productionURL = "https://www.altkeydev.ca/dev/tour_martello_ar/insert.php";
        private bool _surveySent = false;

        public GameObject _endMsg;
        public GameObject _blocker;

        // Survey elements are referenced individually to keep the form's ordering
        public SurveyRadio _accessibility;
        public SurveyRadio _courtesy;
        public SurveyRadio _information;
        public SurveyRadio _price;
        public SurveyRadio _language;
        [Space]
        public SurveyToggle _heardAbout;
        public SurveyText _heardAboutOther;
        [Space]
        public SurveyRadio _age;
        public SurveyRadio _group;
        public SurveyText _residence;
        public SurveyRadio _newsletter;
        public SurveyText _name;
        public SurveyText _email;

        public void Submit()
        {
            // Only send the survey once, to prevent double clicks and stuff
            if (_surveySent)
            {
                return;
            }
            _blocker.SetActive(true);

            // Get the data from all elements and compile it into a readablejson
            /*SurveyAnswers answer = new SurveyAnswers();
            answer._langType = LocalizationManager._lang == LocalizationManager.LangType.EN ? 0 : 1;
            answer._date = System.DateTime.Now.ToShortDateString();
            answer._timeInApp = (int)AppFlowManager._timeInApp;

            answer._accessibility = _accessibility._selectedOption;
            answer._welcome = _welcome._selectedOption;
            answer._activity = _activity._selectedOption;
            answer._price = _price._selectedOption;
            answer._language = _language._selectedOption;
            answer._heardAbout = new bool[_heardAbout._toggledIds.Length];
            for (int i = 0; i < _heardAbout._toggledIds.Length; i++)
            {
                answer._heardAbout[i] = _heardAbout._toggledIds[i];
            }
            answer._heardAboutOther = _heardAboutOther._input;
            answer._age = _age._selectedOption;
            answer._residence = _residence._input;
            answer._newsletter = _newsletter._selectedOption;
            answer._name = _name._input;
            answer._email = _email._input;

            string answerJson = JsonUtility.ToJson(answer);
            Debug.Log(answerJson);
            StartCoroutine(Upload(answerJson));*/

            WWWForm form = new WWWForm();

            form.AddField("ALTKEYTOKEN", 0);
            form.AddField("LANGTYPE", LocalizationManager._lang == LocalizationManager.LangType.EN ? 0 : 1);
            form.AddField("DATE", System.DateTime.Now.ToShortDateString());
            Debug.Log(AppFlowManager._timeInApp);
            form.AddField("TIMEINAPP", (int)Mathf.Round(AppFlowManager._timeInApp));

            form.AddField("ACCESSIBILITY", _accessibility._selectedOption);
            form.AddField("COURTESY", _courtesy._selectedOption);
            form.AddField("INFORMATION", _information._selectedOption);
            form.AddField("PRICE", _price._selectedOption);
            form.AddField("LANGUAGE", _language._selectedOption);
            string heardAbout = "";
            for (int i = 0; i < _heardAbout._toggledIds.Length; i++)
            {
                heardAbout += _heardAbout._toggledIds[i] ? "1" : "0";
            }
            form.AddField("HEARDABOUT", heardAbout);
            form.AddField("HEARDABOUTOTHER", _heardAboutOther._input);
            form.AddField("AGE", _age._selectedOption);
            form.AddField("GROUP1", _age._selectedOption);
            form.AddField("RESIDENCE", _residence._input);
            form.AddField("NEWSLETTER", _newsletter._selectedOption);
            form.AddField("NAME", _name._input);
            form.AddField("EMAIL", _email._input);

            StartCoroutine(Upload(form));

        }

        public IEnumerator Upload(string answer)
        {
            UnityWebRequest www = UnityWebRequest.PostWwwForm(_productionURL, answer);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
            _blocker.SetActive(false);
            _endMsg.SetActive(true);
        }

        private string Authenticate(string username, string password)
        {
            string auth = username + ":" + password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }

        IEnumerator Upload(WWWForm form)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(_productionURL, form))
            {
                www.SetRequestHeader("AUTHORIZATION", Authenticate("PlainesAbraham", "tour_martello_ar"));

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
                _blocker.SetActive(false);
                _endMsg.SetActive(true);
            }
        }
    }
}
