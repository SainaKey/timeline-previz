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
        private bool beforeFrameCanOverlap = false;
        public bool canOverlap = false;

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

        private void Update()
        {
            if (beforeFrameCanOverlap != canOverlap)
                UpdateClipCanoverlapSetting();

            beforeFrameCanOverlap = canOverlap;
        }

        private void UpdateClipCanoverlapSetting()
        {
            foreach (var clip in clips)
            {
                clip.canOverlap = canOverlap;
            }
        }

        public void CreateClip(float startTime, float duration)
        {
            var clipObj = Instantiate(clipPrefab,clipLineRect,false);
            //var clipObj = Instantiate(clipPrefab,Vector3.zero, Quaternion.identity,clipLineRect);

            var clip = clipObj.GetComponent<Clip>();
            clip.canOverlap = canOverlap;
            clip.SetClipPosFromTime(startTime,duration);
            //clip.SetClipPosFromTime(startTime,duration);
            //ìÒâÒåƒÇ‘Ç∆ê≥ÇµÇ≠ìÆÇ≠ÅH
            
            clips.Add(clip);
        }

        public void DeleteClip(Clip clip)
        {
            var clipListTmp = new List<Clip>(clips);
            clipListTmp.Remove(clip);
            clips = clipListTmp;
            //clips.Remove(clip);
            Destroy(clip.gameObject);
        }
    }
}
