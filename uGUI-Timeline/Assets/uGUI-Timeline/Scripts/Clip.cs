using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    [System.Serializable]
    public class ClipData
    {
        public string guidStr;
    }
    
    public class Clip : MonoBehaviour, IDragHandler
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private Track track;
        private RectTransform trackRect;
        [Space]
        [SerializeField] private float startTime;
        [SerializeField] private float endTime;
        [SerializeField] private float clipLengthOfTime;
        
        [SerializeField] private float posXRatio;//clipPosX/trackWidth
        [SerializeField] private float widthRatio;//clipWidth/trackWidth

        [Space] 
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform clipRect;
        [SerializeField] private RectTransform clipLineRect;

        [Space] 
        [SerializeField] private ClipData clipData;
        [SerializeField] private UnityEvent<ClipData> onStartClip;
        [SerializeField] private UnityEvent<ClipData> duringClip;
        [SerializeField] private UnityEvent<ClipData> onEndClip;
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
                onStartClip.Invoke(clipData);
            }
        }

        private void During()
        {
            duringClip.Invoke(clipData);
        }

        private void OnEnd()
        {
            if (isStart)
            {
                isStart = false;
                onEndClip.Invoke(clipData);
            }
        }
        
        private void Start()
        {
            trackRect = track.GetComponent<RectTransform>();
            SetClipLengthOfTime();
            SetClipStartEndTime();
        }

        private void Update()
        {
            BeConsistentRectRatio();
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
            
            posXRatio = clipRect.localPosition.x / trackRect.rect.width;
            widthRatio = clipRect.sizeDelta.x / trackRect.rect.width;
            Debug.Log(trackRect.rect.width);
        }

        private float beforeTrackWidth = 0;
        private void BeConsistentRectRatio()
        {
            //Trackを監視して、サイズが変更されたら比を元に良い感じにする
            if (beforeTrackWidth != trackRect.rect.width)
            {
                Debug.Log("is Changed");
                //pxr = x / width;
                //x = pxr * width;
                var rect = trackRect.rect;
                var x = posXRatio * rect.width;
                var width = widthRatio * rect.width;
                var pos = clipRect.localPosition;
                pos.x = x;
                clipRect.localPosition = pos;
                var sizeDelta = clipRect.sizeDelta;
                sizeDelta.x = width;
                clipRect.sizeDelta = sizeDelta;
            }
            beforeTrackWidth = trackRect.rect.width;
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
