using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UGUITimeline
{
    public class SelectClipGuid : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text text;
        [SerializeField] private Track track;

        private void Update()
        {
            foreach (var clip in track.Clips)
            {
                if (clip.IsSelect)
                    text.text = clip.ClipData.GuidStr;
            }
        }
    }
}
