using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGUITimeline
{
    public class SampleEvent : MonoBehaviour
    {
        [SerializeField] private GameObject sampleCube;
        private void Awake()
        {
            var sampleCubeRoot = GameObject.Find("SampleCubeRoot");
            sampleCube = sampleCubeRoot.transform.Find("SampleCube").gameObject;
        }

        public void OnStart()
        {
            sampleCube.SetActive(true);
        }

        public void During()
        {
            sampleCube.SetActive(true);
        }

        public void OnEnd()
        {
            sampleCube.SetActive(false);
        }
    }
}
