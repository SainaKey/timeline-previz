using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UGUITimeline
{
    [System.Serializable]
    public class ClipData
    {
        public string guidStr;
    }
    
    public class Clip : MonoBehaviour, IBeginDragHandler , IDragHandler , IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler
    {
        [SerializeField] private Timeline timeline;
        [Space]
        [Header("DebugView")]
        [SerializeField] private float startTime;
        [SerializeField] private float endTime;
        [SerializeField] private float clipLengthOfTime;

        [Space] 
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform clipRect;
        [SerializeField] private RectTransform clipLineRect;
        [SerializeField] private RectTransform tracksScalerRect;
        [SerializeField] private Outline outline;

        [Space] 
        [SerializeField] private ClipData clipData;
        [SerializeField] private UnityEvent<ClipData> onStartClip;
        [SerializeField] private UnityEvent<ClipData> duringClip;
        [SerializeField] private UnityEvent<ClipData> onEndClip;
        [SerializeField] private UnityEvent<ClipData> onSelect;
        private bool isStart = false;
        private bool isEnterPointer = false;

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
        
        private void Update()
        {
            SetClipLengthOfTime();
            SetClipStartEndTime();
            TryUnselect();
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
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Select();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                var deltaPos = new Vector3(eventData.delta.x, eventData.delta.y, 0) / canvas.scaleFactor / tracksScalerRect.localScale.x;
                var pos = clipRect.localPosition;
                pos.x += deltaPos.x;
                clipRect.localPosition = pos;
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }

        private void Select()
        {
            outline.enabled = true;
            onSelect.Invoke(clipData);
        }

        private void TryUnselect()
        {
            if (!isEnterPointer && Input.GetMouseButtonDown(0))
                outline.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isEnterPointer = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isEnterPointer = false;
        }

        
    }
}
