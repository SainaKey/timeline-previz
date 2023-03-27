using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UGUITimeline
{
    public class DeleteButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Track track;
        
        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(() => DeleteClip());
        }

        private void DeleteClip()
        {
            foreach (var clip in track.Clips)
            {
                if(clip.IsSelect)
                    track.DeleteClip(clip);
            }
            
        }
    }
}
