using System;
using Cinemachine;
using UnityEngine;

namespace My_Assets.BarScene.Controllers.DrunknessEffect
{
    public class DrunkCameraExtension : CinemachineExtension
    {
        private Func<bool> isEnabled;
        private Func<float> getAmount;
        private Func<float> getSpeed;

        public void Initialize(Func<bool> enabled, Func<float> amount, Func<float> speed)
        {
            isEnabled = enabled;
            getAmount = amount;
            getSpeed = speed;
        }

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (isEnabled == null || !isEnabled()) return;
            if (stage != CinemachineCore.Stage.Finalize) return;

            float wobble = Mathf.Sin(Time.time * getSpeed()) * getAmount();
            Quaternion rotation = Quaternion.Euler(0, 0, wobble);
            state.RawOrientation *= rotation;
        }
    }
}
