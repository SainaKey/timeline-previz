using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UGUITimeline
{
    [System.Serializable]
    public class ClipData
    {
        public string GuidStr;
        public float Progress;
    }
    
    public class Clip : MonoBehaviour, IDragHandler , ISelectable
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private Track track;
        private RectTransform trackRect;
        [SerializeField] private Outline outline;
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
        [SerializeField] public UnityEvent<ClipData> onSelect;
        [SerializeField] public UnityEvent<ClipData> onStartClip;
        [SerializeField] public UnityEvent<ClipData> duringClip;
        [SerializeField] public UnityEvent<ClipData> onEndClip;
        private bool isStart = false;
        [SerializeField] private bool isSelect = false;

        public ClipData ClipData
        {
            get { return this.clipData; }
        }
        public bool IsSelect
        {
            get { return this.isSelect; }
        }

        public void SetCurrentTime(float time)
        {
            if (time >= startTime && endTime >= time)
            {
                OnStart();
                During(time);
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
                clipData.Progress = 0.0f;
                onStartClip.Invoke(clipData);
            }
        }

        private void During(float time)
        {
            
            var progress = (time - startTime) / clipLengthOfTime;
            clipData.Progress = progress;
            duringClip.Invoke(clipData);
        }

        private void OnEnd()
        {
            if (isStart)
            {
                isStart = false;
                clipData.Progress = 1.0f;
                onEndClip.Invoke(clipData);
            }
        }

        private void Awake()
        {
            InitClip();
        }
        
        private void Start()
        {
            SetClipLengthOfTime();
            SetClipStartEndTime();
        }

        private void Update()
        {
            BeConsistentRectRatio();
            SetClipLengthOfTime();
            SetClipStartEndTime();
        }

        private void InitClip()
        {
            timeline = GetComponentInParent<Timeline>();
            track = GetComponentInParent<Track>();
            canvas = GetComponentInParent<Canvas>();
            clipLineRect = track.ClipLineRect;
            clipData.GuidStr = Guid.NewGuid().ToString("N");
            clipData.Progress = 0.0f;
            trackRect = track.GetComponent<RectTransform>();
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
            
            posXRatio = clipRect.localPosition.x / trackRect.rect.width;
            widthRatio = clipRect.sizeDelta.x / trackRect.rect.width;
            //Debug.Log(trackRect.rect.width);
        }

        public void SetClipPosFromTime(float startT, float duration)
        {
            //Debug.Log(startT);
            /*
            if (startT > timeline.LengthOfTime)
            {
                Debug.LogError("The starttime of the clip is longer than the length of the timeline.");
            }

            if (startT + duration > timeline.LengthOfTime)
            {
                Debug.LogError("The endtime of the clip is longer than the length of the timeline.");
            }
            */

            float wholeWidth = clipLineRect.rect.width;//�S�̂̒���
            
            var startRatio = startT / timeline.LengthOfTime;
            var startXPos = startRatio * wholeWidth;
            var anchoredPos = clipRect.anchoredPosition;
            anchoredPos.x = startXPos  + (clipRect.sizeDelta.x / 2.0f);
            clipRect.anchoredPosition = anchoredPos;

            var sizeDelta = clipRect.sizeDelta;
            sizeDelta.x = duration * clipLineRect.rect.width / timeline.LengthOfTime;
            clipRect.sizeDelta = sizeDelta;
        }

        private float beforeTrackWidth = 0;
        private void BeConsistentRectRatio()
        {
            //Track���Ď����āA�T�C�Y���ύX���ꂽ�������ɗǂ������ɂ���
            if (beforeTrackWidth != trackRect.rect.width)
            {
                //Debug.Log("is Changed");
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
            
            var ct = CheckClipTouch2Track();
            if (ct.isLeftTouch)
            {
                if (deltaPos.x < 0)
                    return;
            }
            if (ct.isRightTouch)
            {
                if (deltaPos.x > 0)
                    return;
            }
            
            pos.x += deltaPos.x;
            clipRect.localPosition = pos;
        }

        public (bool isLeftTouch, bool isRightTouch) CheckClipTouch2Track()
        {
            Vector3[] clipWorldCorners = new Vector3[4];
            clipRect.GetWorldCorners(clipWorldCorners);
            
            Vector3[] trackWorldCorners = new Vector3[4];
            trackRect.GetWorldCorners(trackWorldCorners);

            var clipLeft = clipWorldCorners[1];
            var clipRight = clipWorldCorners[3];
            
            var trackLeft = trackWorldCorners[1];
            var trackRight = trackWorldCorners[3];
            
            (bool isLeftTouch, bool isRightTouch) result = (false, false);
            if (clipLeft.x < trackLeft.x)
                result.isLeftTouch = true;
            if (clipRight.x > trackRight.x)
                result.isRightTouch = true;

            return result;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }

        public void Select()
        {
            foreach (var c in track.Clips)
            {
                if(c.IsSelect)
                    c.UnSelect();
            }
            
            outline.enabled = true;
            isSelect = true;
            onSelect.Invoke(clipData);
            Debug.Log(clipData.GuidStr);
        }

        public void UnSelect()
        {
            outline.enabled = false;
            isSelect = false;
        }
    }
}
