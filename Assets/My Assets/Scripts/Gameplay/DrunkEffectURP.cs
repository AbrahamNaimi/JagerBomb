using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace My_Assets.Gameplay
{
    public class DrunkEffectURP : MonoBehaviour
    {
        [Header("Post-Processing")]
        public Volume volume;
        private LensDistortion lens;
        private ChromaticAberration chroma;

        [Header("Camera Wobble")]
        public CinemachineVirtualCamera virtualCamera;
        public bool wobbleEnabled = true;
        public float wobbleAmount = 5f;
        public float wobbleSpeed = 2f;

        private float timer;

        void Start()
        {
            if (volume != null)
            {
                volume.profile.TryGet(out lens);
                volume.profile.TryGet(out chroma);
            }

            if (virtualCamera != null)
            {
           
                if (virtualCamera.GetComponent<DrunkCameraExtension>() == null)
                {
                    virtualCamera.gameObject.AddComponent<DrunkCameraExtension>();
                }

                virtualCamera.GetComponent<DrunkCameraExtension>().Initialize(() => wobbleEnabled, () => wobbleAmount, () => wobbleSpeed);
            }
        }

        void Update()
        {
            timer += Time.deltaTime;

            // Post-processing effecten
            if (lens != null)
                lens.intensity.value = Mathf.Sin(timer * 2f) * 0.3f;

            if (chroma != null)
                chroma.intensity.value = Mathf.Abs(Mathf.Sin(timer * 1.2f)) * 0.5f;
        }
        public void SetWobbleActive(bool active)
        {
            wobbleEnabled = active;
        }
    }
}
