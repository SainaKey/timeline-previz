using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUITimeline
{
    public class IntermediateRayRange : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private List<Track> tracks;
        private GameObject obj;
        
        private void Update()
        {
            if (eventSystem.IsPointerOverGameObject())
            {
                var scroll = Input.mouseScrollDelta.y;
                if (scroll > 0)
                {
                   //Debug.Log("Šg‘å");
                   ExpansionTracksUI();
                }
                else if(scroll < 0)
                {
                    //Debug.Log("k¬");
                    ShrinkTracksUI();
                }
            }
        }

        private void ExpansionTracksUI()
        {
            foreach (var track in tracks)
            {
                var rectTrans = track.GetComponent<RectTransform>();
                var sizeDelta = rectTrans.sizeDelta;
                sizeDelta.x -= 10;
                rectTrans.sizeDelta = sizeDelta;
            }
        }

        private void ShrinkTracksUI()
        {
            foreach (var track in tracks)
            {
                var rectTrans = track.GetComponent<RectTransform>();
                var sizeDelta = rectTrans.sizeDelta;
                sizeDelta.x += 10;
                rectTrans.sizeDelta = sizeDelta;
            }
        }
    }
}
