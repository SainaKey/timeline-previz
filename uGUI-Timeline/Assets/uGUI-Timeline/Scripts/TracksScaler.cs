using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class TracksScaler : MonoBehaviour , IPointerEnterHandler ,IPointerExitHandler , IDragHandler
    {
        private float defaultTracksScale;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform trackScalerRect;
        private bool isPointerEnter = false;
        [SerializeField] private RectTransform sliderHandleRect;
        [SerializeField] private float scaleMax;
        [SerializeField] private float scaleMin;

        private void Start()
        {
            defaultTracksScale = trackScalerRect.localScale.x;
        }

        private Vector3 currentScale = Vector3.zero;
        private void Update()
        {
            var scroll = Input.mouseScrollDelta.y;
            if (isPointerEnter)
            {
                currentScale = trackScalerRect.localScale;
                if (scroll > 0)
                {
                    Debug.Log("上スクロール");
                    currentScale.x += 0.1f;
                    trackScalerRect.localScale = currentScale;
                }
                else if(scroll < 0)
                {
                    Debug.Log("下スクロール");
                    currentScale.x -= 0.1f;
                    trackScalerRect.localScale = currentScale;
                }

                if (currentScale.x > scaleMax)
                {
                    currentScale.x = scaleMax;
                    trackScalerRect.localScale = currentScale;
                }
                
                if (currentScale.x < scaleMin)
                {
                    currentScale.x = scaleMin;
                    trackScalerRect.localScale = currentScale;
                }

                sliderHandleRect.localScale = new Vector2(defaultTracksScale / trackScalerRect.localScale.x, sliderHandleRect.localScale.y);
            }
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerEnter = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                var deltaPos = new Vector3(eventData.delta.x, eventData.delta.y, 0) / canvas.scaleFactor;
                var pos = trackScalerRect.localPosition;
                pos.x += deltaPos.x;
                trackScalerRect.localPosition = pos;
            }
        }
    }
}
