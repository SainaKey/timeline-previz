using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UGUITimeline
{
    public class ClipDebugUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        public void SetText(ClipData clipData)
        {
            text.text = clipData.Progress.ToString();
        }
    }
}
