using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class ClipResizer : MonoBehaviour , IDragHandler
    {
        [SerializeField] private bool isRight;
        [Space]
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform clipRectTrans;
        public void OnDrag(PointerEventData eventData)
        {
            /*
            clipRectTrans.anchorMin = Vector2.zero;
            clipRectTrans.anchorMax = Vector2.one;
            */

            //var pos = edge.localPosition;
            var deltaPos = new Vector3(eventData.delta.x, eventData.delta.y, 0) / canvas.scaleFactor;
            //edge.localPosition = pos + deltaPos;
            if (isRight)
            {
                var sizeDelta = clipRectTrans.sizeDelta;
                sizeDelta.x += (deltaPos.x * 2.0f / 3.0f);
                clipRectTrans.sizeDelta = sizeDelta;

                var localPos = clipRectTrans.localPosition;
                localPos.x += (deltaPos.x * 1.0f / 3.0f);
                clipRectTrans.localPosition = localPos;
            }
            else
            {
                var sizeDelta = clipRectTrans.sizeDelta;
                sizeDelta.x -= (deltaPos.x * 2.0f / 3.0f);
                clipRectTrans.sizeDelta = sizeDelta;

                var localPos = clipRectTrans.localPosition;
                localPos.x += (deltaPos.x * 1.0f / 3.0f);
                clipRectTrans.localPosition = localPos;
            }
            
            
        }
        
        
    }
}
