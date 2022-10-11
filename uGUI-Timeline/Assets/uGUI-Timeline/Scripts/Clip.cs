using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGUITimeline
{
    public class Clip : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        [Space]
        [SerializeField] private float startTime;
        [SerializeField] private float endTime;
        [SerializeField] private float clipLengthOfTime;

        [Space] [SerializeField] 
        private RectTransform clipRect;
        [SerializeField] private RectTransform clipLineRect;

        [Space] [Header("Debug")] 
        [SerializeField] private GameObject debugObj;
        
        public void SetCurrentTime(float time)
        {
            if (time >= startTime && endTime >= time)
                OnActive();
            else 
                OnInactive();
        }
        
        private void Update()
        {
            SetClipLengthOfTime();
            SetClipStartEndTime();
        }

        private void SetClipLengthOfTime()
        {
            //全体の時間 : クリップの長さ　= cliplineRectの長さ : このUIの長さ
            //lengthOfTime : clipLengthOfTime = clipLineRect.sizeDelta.y : clipRect.sizedelta.y
            //clipLengthOfTime * clipLineRect.sizeDelta.y = lengthOfTime * clipRect.sizedelta.y
            clipLengthOfTime = (timeline.LengthOfTime * clipRect.sizeDelta.x) / clipLineRect.rect.width;
        }

        private void SetClipStartEndTime()
        {
            //startが0:-width/2がx
            //startが最後:width + width /2
            //x - -width/2がxの値が全体の何割か
            
            //endが0:-width - width /2
            //endが最後:width/2 
            //x - -width - width /2 がxの値が全体の何割か？
            
            float wholeWidth = clipLineRect.rect.width;//全体の長さ
            float startXPos = clipRect.anchoredPosition.x - (clipRect.sizeDelta.x / 2.0f);
            //float endXPos = clipRect.localPosition.x - (-clipRect.sizeDelta.x - (clipRect.sizeDelta.x / 2.0f));
            
            var startRatio = startXPos / wholeWidth;
            //var endRatio = endXPos / wholeWidth;

            startTime = timeline.LengthOfTime * startRatio;
            endTime = startTime + clipLengthOfTime;
        }

        

        private void OnActive()
        {
            debugObj.SetActive(true);
        }

        private void OnInactive()
        {
            debugObj.SetActive(false);
        }
    }
}
