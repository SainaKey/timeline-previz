using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGUITimeline
{
    public class DebugCube : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("x²‚Ì‰ñ“]Šp“x")]
        private float rotateX = 0;
    
        [SerializeField]
        [Tooltip("y²‚Ì‰ñ“]Šp“x")]
        private float rotateY = 0;

        [SerializeField]
        [Tooltip("z²‚Ì‰ñ“]Šp“x")]
        private float rotateZ = 0;

        // Update is called once per frame
        void Update()
        {
            // X,Y,Z²‚É‘Î‚µ‚Ä‚»‚ê‚¼‚êAw’è‚µ‚½Šp“x‚¸‚Â‰ñ“]‚³‚¹‚Ä‚¢‚éB
            // deltaTime‚ğ‚©‚¯‚é‚±‚Æ‚ÅAƒtƒŒ[ƒ€‚²‚Æ‚Å‚Í‚È‚­A1•b‚²‚Æ‚É‰ñ“]‚·‚é‚æ‚¤‚É‚µ‚Ä‚¢‚éB
            gameObject.transform.Rotate(new Vector3(rotateX, rotateY, rotateZ) * Time.deltaTime);
        }

        public void StartActive()
        {
            Debug.Log("start");
            gameObject.SetActive(true);
        }

        public void UpdateActive()
        {
            Debug.Log("update");
        }

        public void EndActive()
        {
            Debug.Log("end");
            gameObject.SetActive(false);
        }
    }
}
