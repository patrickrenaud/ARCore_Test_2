using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class LocalizationLinker : MonoBehaviour
    {

        // Start is called before the first frame update
        public void ToggleLanguage()
        {
            LocalizationManager._instance.ToggleLanguage();
        }

        public void SetLanguage(string lang)
        {
            LocalizationManager._instance.SetLanguage(lang);
        }
    }
}
