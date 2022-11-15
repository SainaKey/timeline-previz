using UnityEngine.Playables;

namespace dev.kemomimi.TimelineExtension.AbstractValueControlTrack
{
    public class AbstractFloatValueControlMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var trackBinding = playerData as AbstractFloatValueController;

            if (!trackBinding)
                return;

            var value = 0f;

            var inputCount = playable.GetInputCount();

            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<AbstractFloatValueControlBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                if (inputWeight == 0f)
                {
                    continue;
                }

                value += inputBehaviour.Value * inputWeight;
            }

            //Apply
            trackBinding.SetValue(value);
        }
    }
}