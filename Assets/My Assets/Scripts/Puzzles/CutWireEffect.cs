using UnityEngine;

namespace My_Assets.Puzzles
{
    public class CutFlashEffect : MonoBehaviour
    {
        public float duration = 0.5f;

        private LineRenderer _lineRenderer;
        private Color _startColor;
        private Color _endColor;
        private float _elapsed;
        public bool shouldFade = false;

        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            if (_lineRenderer == null)
            {
                Debug.LogError("CutFlashEffect: Missing LineRenderer");
                return;
            }

            _lineRenderer.useWorldSpace = true;
            _lineRenderer.positionCount = 2;

            _lineRenderer.startWidth = 0.2f;
            _lineRenderer.endWidth = 0.2f;

            _lineRenderer.material.color = Color.magenta;

            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, transform.position + transform.up * 1.5f);

            _startColor = Color.magenta;
            _endColor = Color.magenta;

            _elapsed = 0f;
        }

        void Update()
        {
            if (!shouldFade) return;

            _elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, _elapsed / duration);

            _lineRenderer.startColor = new Color(_startColor.r, _startColor.g, _startColor.b, alpha);
            _lineRenderer.endColor = new Color(_endColor.r, _endColor.g, _endColor.b, alpha);

            if (_elapsed >= duration)
            {
                Destroy(gameObject);
            }
        }
    }
}