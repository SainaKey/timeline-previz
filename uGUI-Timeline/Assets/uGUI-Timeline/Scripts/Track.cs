using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UGUITimeline
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private List<Clip> clips;

        public void SetCurrentTime(float currentTIme)
        {
            foreach (var clip in clips)
            {
                clip.SetCurrentTime(currentTIme);
            }
        }
    }
}
