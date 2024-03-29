using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGUITimeline
{
    public class DebugCube : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("x軸の回転角度")]
        private float rotateX = 0;
    
        [SerializeField]
        [Tooltip("y軸の回転角度")]
        private float rotateY = 0;

        [SerializeField]
        [Tooltip("z軸の回転角度")]
        private float rotateZ = 0;

        // Update is called once per frame
        void Update()
        {
            // X,Y,Z軸に対してそれぞれ、指定した角度ずつ回転させている。
            // deltaTimeをかけることで、フレームごとではなく、1秒ごとに回転するようにしている。
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
