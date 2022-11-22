using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UGUITimeline
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] private float lengthOfTime;
        [SerializeField] private bool isPlay;
        [Space]
        [SerializeField] private float currentTime = 0f;
        [SerializeField] private TMP_InputField lengthOfTimeInputField;
        [SerializeField] private Slider seekSlider;
        [SerializeField] private Toggle playToggle;
        [SerializeField] private TimeLineBackRaycastTarget timeLineBackRaycastTarget;
        [SerializeField] private List<Track> tracks;

        public float LengthOfTime
        {
            get { return this.lengthOfTime; }
        }

        public List<Track> Tracks
        {
            get { return this.tracks; }
        }
        private void Start()
        {
            lengthOfTimeInputField.text = lengthOfTime.ToString();
            seekSlider.maxValue = lengthOfTime;
            playToggle.isOn = isPlay;
            
            lengthOfTimeInputField.onEndEdit.AddListener(tex => SetLengthOfTime(tex));
            seekSlider.onValueChanged.AddListener(value => OnSeekSliderChanged(value));
            playToggle.onValueChanged.AddListener(value => OnPlayToggleChanged(value));

            timeLineBackRaycastTarget.Tracks = tracks;
        }

        private void Update()
        {
            if (isPlay)
            {
                if (currentTime >= lengthOfTime)
                    currentTime = 0;
            
                currentTime += Time.deltaTime;

                seekSlider.value = currentTime;
                foreach (var track in tracks)
                {
                    track.SetCurrentTime(currentTime);
                }
            }
        }

        private void SetLengthOfTime(string tex)
        {
            if (float.TryParse(tex, out float value))
            {
                lengthOfTime = value;
                seekSlider.maxValue = lengthOfTime;
            }
        }

        private void OnSeekSliderChanged(float value)
        {
            currentTime = value;
            foreach (var track in tracks)
            {
                track.SetCurrentTime(currentTime);
            }
        }

        private void OnPlayToggleChanged(bool b)
        {
            isPlay = b;
        }

        public void CreateClip(Track targetTrack, float startTime, float duration)
        {
            targetTrack.CreateClip(startTime,duration);
        }
    }
}
