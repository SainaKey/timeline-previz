using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class TimeLineBackRaycastTarget : MonoBehaviour
    {
        [SerializeField] private RectTransform tracksRect;
        [SerializeField] private float scaleDelta;
        [SerializeField] private EventSystem eventSystem; 
        private List<Track> tracks;
        
        public List<Track> Tracks
        {
            set { tracks = value; }
        }

        private void Start()
        {
            if (scaleDelta <= 0.0f)
            {
                scaleDelta = 10;
            }
        }

        private void Update()
        {
            if (eventSystem.IsPointerOverGameObject())
            {
                var scroll = Input.mouseScrollDelta.y;
                if (scroll > 0)
                {
                    ShrinkTracksUI();
                }
                else if(scroll < 0)
                {
                    ExpansionTracksUI();
                    
                }
            }
        }

        private void ExpansionTracksUI()
        {
            /*
            foreach (var track in tracks)
            {
                var rectTrans = track.GetComponent<RectTransform>();
                var sizeDelta = rectTrans.sizeDelta;
                sizeDelta.x -= scaleDelta;
                rectTrans.sizeDelta = sizeDelta;
            }
            */
            
            var sizeDelta = tracksRect.sizeDelta;
            sizeDelta.x -= scaleDelta;
            tracksRect.sizeDelta = sizeDelta;
        }

        private void ShrinkTracksUI()
        {
            /*
            foreach (var track in tracks)
            {
                var rectTrans = track.GetComponent<RectTransform>();
                var sizeDelta = rectTrans.sizeDelta;
                sizeDelta.x += scaleDelta;
                rectTrans.sizeDelta = sizeDelta;
            }
            */
            var sizeDelta = tracksRect.sizeDelta;
            sizeDelta.x += scaleDelta;
            tracksRect.sizeDelta = sizeDelta;
        }
    }
}
