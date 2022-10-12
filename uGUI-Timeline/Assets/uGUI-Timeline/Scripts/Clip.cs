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
            //�S�̂̎��� : �N���b�v�̒����@= cliplineRect�̒��� : ����UI�̒���
            //lengthOfTime : clipLengthOfTime = clipLineRect.sizeDelta.y : clipRect.sizedelta.y
            //clipLengthOfTime * clipLineRect.sizeDelta.y = lengthOfTime * clipRect.sizedelta.y
            clipLengthOfTime = (timeline.LengthOfTime * clipRect.sizeDelta.x) / clipLineRect.rect.width;
        }

        private void SetClipStartEndTime()
        {
            //start��0:-width/2��x
            //start���Ō�:width + width /2
            //x - -width/2��x�̒l���S�̂̉�����
            
            //end��0:-width - width /2
            //end���Ō�:width/2 
            //x - -width - width /2 ��x�̒l���S�̂̉������H
            
            float wholeWidth = clipLineRect.rect.width;//�S�̂̒���
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
