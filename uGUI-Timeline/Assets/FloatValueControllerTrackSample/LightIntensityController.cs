using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dev.kemomimi.TimelineExtension.AbstractValueControlTrack
{
    public class LightIntensityController : AbstractFloatValueController
    {
        [SerializeField] private List<Light> lightList;

        protected override void OnValueChanged(float value)
        {
            foreach (var light in lightList)
            {
                light.intensity = value;
            }
        }
    }
}