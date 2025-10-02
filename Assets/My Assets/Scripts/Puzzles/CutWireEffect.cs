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
        public bool shouldFade = true;

        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.up * 0.2f);

            _startColor = _lineRenderer.startColor;
            _endColor = _lineRenderer.endColor;
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