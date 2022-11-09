using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UGUITimeline
{
    public class CreateButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Timeline timeline;
        [SerializeField] private Track track;

        [SerializeField] private float startTime;

        [SerializeField] private float duration;
        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(() => CreateClip());
        }

        private void CreateClip()
        {
            timeline.CreateClip(track,startTime,duration);   
        }
    }
}
