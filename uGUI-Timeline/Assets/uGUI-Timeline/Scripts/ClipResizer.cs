using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class ClipResizer : MonoBehaviour , IDragHandler , IEndDragHandler , IBeginDragHandler
    {
        [SerializeField] private bool isRight;
        [Space] 
        [SerializeField] private Camera camera;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Clip clip;
        [SerializeField] private RectTransform clipRectTrans;
        private Vector3 lastPos = Vector3.zero;
        private Vector2 lastSizeDelta = Vector2.zero;

        private bool isMaxFirst = true;
        private Vector3 maxLocalPos = Vector3.zero;
        private Vector2 maxSizeDelta = Vector2.zero;

        private void Start()
        {
            //ref:https://light11.hatenadiary.com/entry/2019/04/16/003642
            canvas = GetComponentInParent<Canvas>();
            
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                camera = null;
            }
            else
            {
                camera = canvas.worldCamera;
            }
            
            lastPos = clipRectTrans.localPosition;
            lastSizeDelta = clipRectTrans.sizeDelta;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            lastPos = clipRectTrans.localPosition;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            //clipの端のworldPosとマウスのworldPosを得てなんやかんやすると良さそう
            //var deltaPos = new Vector3(delta.x, delta.y, 0) / (canvas.scaleFactor);
            var screenPoint = eventData.position;
            var worldPoint = Vector3.zero;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(clipRectTrans, screenPoint, camera, out worldPoint);
            
            //rectの右上取りたい
            Vector3[] v = new Vector3[4];
            clipRectTrans.GetWorldCorners(v);
            //Debug.Log("右上"+v[3]);

            var ct = clip.CheckClipTouchOnTrack();
            
            if (isRight)
            {
                var deltaPos = v[3].x - worldPoint.x / canvas.scaleFactor;

                if (ct.isRightTouch)
                {
                    if (deltaPos < 0)
                        return;
                }
                
                var sizeDelta = clipRectTrans.sizeDelta;
                sizeDelta.x -= (deltaPos * 2.0f / 3.0f) ;
                clipRectTrans.sizeDelta = sizeDelta;

                var localPos = clipRectTrans.localPosition;
                localPos.x -= (deltaPos * 1.0f / 3.0f) ;
                clipRectTrans.localPosition = localPos;
            }
            else
            {
                var deltaPos = v[0].x - worldPoint.x / canvas.scaleFactor;
                
                if (ct.isLeftTouch)
                {
                    if (deltaPos > 0)
                        return;
                }
                
                var sizeDelta = clipRectTrans.sizeDelta;
                sizeDelta.x += (deltaPos * 2.0f / 3.0f) ;
                clipRectTrans.sizeDelta = sizeDelta;

                var localPos = clipRectTrans.localPosition;
                localPos.x -= (deltaPos * 1.0f / 3.0f) ;
                clipRectTrans.localPosition = localPos;
            }

            if (clip.IsOverlapOtherClip())
            {
                if (isMaxFirst)
                {
                    maxLocalPos = clipRectTrans.localPosition;
                    maxSizeDelta = clipRectTrans.sizeDelta;
                    isMaxFirst = false;
                }
            }
            

        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (clip.IsOverlapOtherClip())
            {
                var snapTarget = clip.GetSnapTarget();
                var sizeDelta = clipRectTrans.sizeDelta;
                if (snapTarget.snapToRight)
                {
                    clipRectTrans.localPosition = maxLocalPos;
                    clipRectTrans.sizeDelta = maxSizeDelta;
                }
                else
                {
                    clipRectTrans.localPosition = maxLocalPos;
                    clipRectTrans.sizeDelta = maxSizeDelta;
                }
            }
            else
            {
                if (clip.IsCrossOverOtherClip())
                {
                    clipRectTrans.localPosition = lastPos;
                    clipRectTrans.sizeDelta = lastSizeDelta;
                }

            }
            
            lastPos = clipRectTrans.localPosition;
            lastSizeDelta = clipRectTrans.sizeDelta;
            isMaxFirst = true;
        }

       
    }
}
