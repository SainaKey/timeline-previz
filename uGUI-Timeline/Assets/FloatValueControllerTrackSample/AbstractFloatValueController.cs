using UnityEngine;

namespace dev.kemomimi.TimelineExtension.AbstractValueControlTrack
{
    public abstract class AbstractFloatValueController : MonoBehaviour
    {
        public float value { get; private set; }

        public void SetValue(float value)
        {
            this.value = value;
            OnValueChanged(this.value);
        }

        protected abstract void OnValueChanged(float value);
    }
}