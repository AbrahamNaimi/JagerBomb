using System.Collections;
using UnityEngine;

namespace My_Assets.BarScene.Controllers
{
    public class Drinkable : MonoBehaviour
    {
        [Header("Timings")]
        public float moveDuration = 1.0f;      // time to move to mouth
        public float sipDuration = 1.0f;      // time to "drink" (pause at mouth)
        public float returnDuration = 1.0f;    // time to return

        [Header("Target")]
        public Transform seatAnchor;           // optional: assign SitSpot.seatAnchor; if null, uses camera

        [Header("Audio")]
        public AudioClip sipSound;              // assign your .mp3/.wav in inspector
        public AudioSource audioSource;         // assign or will be auto-added

        public bool IsBusy { get; private set; }

        void Awake()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
                if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }

        public void Drink(Transform cam)
        {
            if (IsBusy) return;
            StartCoroutine(DrinkRoutine(cam));
        }

        private IEnumerator DrinkRoutine(Transform cam)
        {
            IsBusy = true;

            // capture start
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;

            // pick target (prefer seatAnchor)
            Vector3 targetPos = seatAnchor ? seatAnchor.position : cam.position;
            Quaternion targetRot = seatAnchor ? seatAnchor.rotation : cam.rotation;

            // move to mouth
            yield return LerpPose(startPos, targetPos, startRot, targetRot, moveDuration);

            // sip (play sound & wait)
            if (sipDuration > 0f)
            {
                if (sipSound && audioSource) audioSource.PlayOneShot(sipSound);
                yield return new WaitForSeconds(sipDuration);
            }

            // return to start
            yield return LerpPose(targetPos, startPos, targetRot, startRot, returnDuration);

            IsBusy = false;
        }

        private IEnumerator LerpPose(Vector3 fromPos, Vector3 toPos, Quaternion fromRot, Quaternion toRot, float duration)
        {
            duration = Mathf.Max(0.0001f, duration);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                float s = Mathf.SmoothStep(0f, 1f, t);
                transform.position = Vector3.LerpUnclamped(fromPos, toPos, s);
                transform.rotation = Quaternion.SlerpUnclamped(fromRot, toRot, s);
                yield return null;
            }
            transform.position = toPos;
            transform.rotation = toRot;
        }
    }
}
