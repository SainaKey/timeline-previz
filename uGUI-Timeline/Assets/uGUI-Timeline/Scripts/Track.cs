using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UGUITimeline
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private List<Clip> clips;
        [SerializeField] private RectTransform clipLineRect;
        [SerializeField] private GameObject clipPrefab;

        public List<Clip> Clips
        {
            get { return this.clips; }
        }
        public RectTransform ClipLineRect
        {
            get { return this.clipLineRect; }
        }
        public void SetCurrentTime(float currentTIme)
        {
            foreach (var clip in clips)
            {
                clip.SetCurrentTime(currentTIme);
            }
        }
        
        public void CreateClip(float startTime, float duration)
        {
            var clipObj = Instantiate(clipPrefab,clipLineRect,false);
            //var clipObj = Instantiate(clipPrefab,Vector3.zero, Quaternion.identity,clipLineRect);

            var clip = clipObj.GetComponent<Clip>();
            clip.SetClipPosFromTime(startTime,duration);
            clip.SetClipPosFromTime(startTime,duration);
            //二回呼ぶと正しく動く、なぜ...
            
            clips.Add(clip);
        }

        public void DeleteClip(Clip clip)
        {
            clips.Remove(clip);
            Destroy(clip.gameObject);
        }
    }
}
