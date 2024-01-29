using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ca.altkey
{
    public class VisitedArTrackers : MonoBehaviour
    {
        public void AddVisitedMarker(int index)
        {
            WindowManager._visitedItem[index] = true;
        }
    }
}