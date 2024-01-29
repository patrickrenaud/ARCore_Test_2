using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class SurveyToggle : MonoBehaviour
    {
        // In this case, the only set of toggles has 11 elements
        public BitArray _toggledIds = new BitArray(13, false);

        public void ToggleId(int id)
        {
            _toggledIds[id] = !_toggledIds[id];
        }
    }
}
