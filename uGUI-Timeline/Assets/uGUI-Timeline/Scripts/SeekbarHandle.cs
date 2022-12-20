using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class SeekbarHandle : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
    {
        public UnityEvent onSeekbarClick = new UnityEvent();
        public UnityEvent onSeekbarRelease = new UnityEvent();
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Down");
            onSeekbarClick.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Up");
            onSeekbarRelease.Invoke();
        }
    }
}
