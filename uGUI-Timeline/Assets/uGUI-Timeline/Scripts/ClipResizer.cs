using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class ClipResizer : MonoBehaviour , IDragHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform clipRectTrans;
        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log(eventData.delta);
            var sizeDelta = clipRectTrans.sizeDelta;
            sizeDelta.x += eventData.delta.x / 2.0f /canvas.scaleFactor ;
            clipRectTrans.sizeDelta = sizeDelta;

            var localPos = clipRectTrans.localPosition;
            localPos.x += eventData.delta.x / 2.0f /canvas.scaleFactor;
            clipRectTrans.localPosition = localPos;
        }
    }
}
