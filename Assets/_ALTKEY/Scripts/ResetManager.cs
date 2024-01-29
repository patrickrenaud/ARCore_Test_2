using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ca.altkey
{
    public class ResetManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            WindowManager._visitedItem = new List<bool>();
            WindowManager._currentMode = WindowManager.WindowMode.MAP;
            WindowManager._previousMode = WindowManager.WindowMode.MAP;
            WindowManager._listScrollPosition = 1f;
            ChangeLevel._levelIndex = 1;
        }
    }
}
