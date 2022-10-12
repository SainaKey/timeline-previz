using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class Clip : MonoBehaviour, IDragHandler
    {
        [SerializeField] private Timeline timeline;
        [Space]
        private float startTime;
        private float endTime;
        private float clipLengthOfTime;

        [Space] 
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform clipRect;
        [SerializeField] private RectTransform clipLineRect;

        [Space] 
        [SerializeField] private UnityEvent onStart;
        [SerializeField] private UnityEvent during;
        [SerializeField] private UnityEvent onEnd;
        private bool isStart = false;

        public void SetCurrentTime(float time)
        {
            if (time >= startTime && endTime >= time)
            {
                OnStart();
                During();
            }
            else
            {
                OnEnd();
            }
        }

        private void OnStart()
        {
            if (!isStart)
            {
                isStart = true;
                onStart.Invoke();
            }
        }

        private void During()
        {
            during.Invoke();
        }

        private void OnEnd()
        {
            if (isStart)
            {
                isStart = false;
                onEnd.Invoke();
            }
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

        
        

        public void OnDrag(PointerEventData eventData)
        {
            var deltaPos = new Vector3(eventData.delta.x, eventData.delta.y, 0) / canvas.scaleFactor;
            var pos = clipRect.localPosition;
            pos.x += deltaPos.x;
            clipRect.localPosition = pos;
        }
    }
}
