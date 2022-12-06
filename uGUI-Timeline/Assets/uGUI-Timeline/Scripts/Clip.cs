using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UGUITimeline
{
    [System.Serializable]
    public class ClipData
    {
        public string GuidStr;
        public float Progress;
    }
    
    public class Clip : MonoBehaviour, IDragHandler , ISelectable , IEndDragHandler
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private Track track;
        private RectTransform trackRect;
        [SerializeField] private Image image;
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
        private bool isMouseOn = false;
        Vector3 lastPos = Vector3.zero;

        [Header("UI")] 
        [SerializeField] private CanvasGroup canvasGroup;

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
            lastPos = trackRect.localPosition;
        }

        private void Update()
        {
            BeConsistentRectRatio();
            SetClipLengthOfTime();
            SetClipStartEndTime();
            if(Input.GetKeyDown(KeyCode.Mouse0))
                if (!isMouseOn)
                {
                    UnSelect();
                }
            
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

            float wholeWidth = clipLineRect.rect.width;//全体の長さ
            
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
            //Trackを監視して、サイズが変更されたら比を元に良い感じにする
            if (beforeTrackWidth != trackRect.rect.width)
            {
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

        public bool IsCrossOverOtherClip()
        {
            bool isCrossOver = false;
            //snap出来る範囲を超えてたら一個前の状態に戻す
            Vector3[] clipWorldCorners = new Vector3[4];
            clipRect.GetWorldCorners(clipWorldCorners);
            
            Vector3[] otherClipWorldCorners = new Vector3[4];

            foreach (var clip in track.Clips)
            {
                if(clip == this)
                    continue;
                    
                clip.clipRect.GetWorldCorners(otherClipWorldCorners);

                var left = otherClipWorldCorners[0];
                var right = otherClipWorldCorners[3];

                var thisLeft = clipWorldCorners[0];
                var thisRight = clipWorldCorners[3];

                if (thisLeft.x < left.x && thisRight.x > right.x)
                    isCrossOver = true;
            }

            return isCrossOver;
        }

        public bool IsOverlapOtherClip()
        {
            bool isOverlap = false;
            //他のclipと重なっているか判定する
            //自分のworldCornersのどれかが相手のrectの中に入ってたら重なってる
            Vector3[] clipWorldCorners = new Vector3[4];
            clipRect.GetWorldCorners(clipWorldCorners);

            Vector3[] otherClipWorldCorners = new Vector3[4];
            
            foreach (var clip in track.Clips)
            {
                if(clip == this)
                    continue;
                    
                clip.clipRect.GetWorldCorners(otherClipWorldCorners);

                var left = otherClipWorldCorners[0];
                var right = otherClipWorldCorners[3];
                foreach (var clipWorldCorner in clipWorldCorners)
                {
                    if (clipWorldCorner.x > left.x && clipWorldCorner.x < right.x)
                        isOverlap = true;
                }
            }

            return isOverlap;
        }

        public (bool snapToRight , RectTransform targetRect) GetSnapTarget()
        {
            //一番近いsnap先を手に入れる
            //自分の右端と相手の左端
            //自分の左端と相手の右端
            //距離が近い方にスナップする
            bool snapToRight = false;
            RectTransform otherClipRect = null;

            Vector3[] clipWorldCorners = new Vector3[4];
            clipRect.GetWorldCorners(clipWorldCorners);
            
            Vector3[] otherClipWorldCorners = new Vector3[4];
            float minDist = float.MaxValue;
            foreach (var track in timeline.Tracks)
            {
                foreach (var clip in track.Clips)
                {
                    if(clip == this)
                        continue;
                    
                    clip.clipRect.GetWorldCorners(otherClipWorldCorners);

                    var left = otherClipWorldCorners[0];
                    var right = otherClipWorldCorners[3];

                    var thisLeft = clipWorldCorners[0];
                    var thisRight = clipWorldCorners[3];

                    //差が0以下の場合は重なってない
                    var dist0 = thisRight.x - left.x;
                    var dist1 = right.x - thisLeft.x;
                    
                    if (dist0 <= 0)
                        dist0 = float.MaxValue;
                    if (dist1 <= 0)
                        dist1 = float.MaxValue;
                        

                    if (dist0 < minDist || dist1 < minDist)
                    {
                        if (dist0 < dist1)
                        {
                            minDist = dist0;
                            snapToRight = false;
                            otherClipRect = clip.clipRect;
                        }
                        else if (dist0 > dist1)
                        {
                            minDist = dist1;
                            snapToRight = true;
                            otherClipRect = clip.clipRect;
                        }
                    }
                }
            }
            return (snapToRight,otherClipRect);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            Select();
            var deltaPos = new Vector3(eventData.delta.x, eventData.delta.y, 0) / canvas.scaleFactor;
            var pos = clipRect.localPosition;
            Debug.Log(pos);
            
            var ct = CheckClipTouchOnTrack();
            if (ct.isLeftTouch)
            {
                if (deltaPos.x < 0)
                {
                    var tmpPos = clipRect.anchoredPosition;
                    tmpPos.x = (clipRect.sizeDelta.x * 1f / 3f) / (2f / 3f);
                    clipRect.anchoredPosition = tmpPos;
                    return;
                }
                    
            }
            if (ct.isRightTouch)
            {
                if (deltaPos.x > 0)
                {
                    var tmpPos = clipRect.anchoredPosition;
                    tmpPos.x = ((clipLineRect.rect.width*2f/3f)-(clipRect.sizeDelta.x * 1f / 3f)) / (2f / 3f);
                    clipRect.anchoredPosition = tmpPos;
                    return;
                }
                    
            }

            pos.x += deltaPos.x;
            clipRect.localPosition = pos;
        }
        
        
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsOverlapOtherClip())
            {
                var snapTarget =  GetSnapTarget();
                var pos = clipRect.localPosition;
                if (snapTarget.snapToRight)
                {
                    //相手の右端にsnap
                    //相手のpos*2/3 +　width*1/3 = 自分のpos*2/3 - width*1/3になれば良い
                    pos.x = (((snapTarget.targetRect.localPosition.x * 2f / 3f) + (snapTarget.targetRect.sizeDelta.x * 1f / 3f)) + (clipRect.sizeDelta.x * 1f / 3f)) / (2f / 3f);
                    //result.x= (otherClipRect.localPosition.x * 2f / 3f + otherClipRect.sizeDelta.x * 1f / 3f) + (clipRect.sizeDelta.x * 1f / 3f) / (2f / 3f);
                }
                else
                {
                    //相手の左端にsnap
                    //自分のpos*2/3 + width * 1/3が相手のpos*2/3 - width * 1/3になれば良い
                    pos.x = ((snapTarget.targetRect.localPosition.x*2f/3f - snapTarget.targetRect.sizeDelta.x * 1f/3f) - (clipRect.sizeDelta.x * 1f/3f)) / (2f/3f);
                }
                clipRect.localPosition = pos;
            }

            if (IsCrossOverOtherClip())
            {
                clipRect.localPosition = lastPos;
            }

            lastPos = clipRect.localPosition;
        }
        
        
        public (bool isLeftTouch, bool isRightTouch) CheckClipTouchOnTrack()
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
            {
                result.isLeftTouch = true;
            }

            if (clipRight.x > trackRight.x)
            {
                result.isRightTouch = true;
            }
            return result;
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("click");
            if (isSelect)
            {
                Debug.Log("is");
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            Select();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOn = false;
        }

        public void Select()
        {
            foreach (var t in timeline.Tracks)
            {
                foreach (var c in t.Clips)
                {
                    if(c == this)
                        continue;
                    
                    if(c.IsSelect)
                        c.UnSelect();
                }
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
            
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
        
    }
}
