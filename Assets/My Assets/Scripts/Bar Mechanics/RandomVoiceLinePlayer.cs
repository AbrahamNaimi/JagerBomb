using System.Collections;
using UnityEngine;

namespace My_Assets.Bar_Mechanics
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomVoiceLinePlayer : MonoBehaviour
    {
        [Header("Voice Lines")]
        public AudioClip[] voiceLines;   // drag your mp3/wav clips here
        public float minDelay = 1f;      // seconds before first line
        public float maxDelay = 3f;

        [Header("Audio")]
        [Range(0f, 1f)] public float volume = 1f;
        [Tooltip("0 = 2D (no falloff), 1 = 3D (spatial). For HUD-style voices, use 0.")]
        [Range(0f, 1f)] public float spatialBlend = 0f;

        AudioSource src;

        void Awake()
        {
            src = GetComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = spatialBlend; // 0 = 2D so it won’t get quieter/farther
        }

        void Start()
        {
            if (voiceLines != null && voiceLines.Length > 0)
                StartCoroutine(PlayOnceAfterDelay());
        }

        IEnumerator PlayOnceAfterDelay()
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            var clip = voiceLines[Random.Range(0, voiceLines.Length)];
            if (clip) src.PlayOneShot(clip, volume);  // follows the camera
        }
    }
}
