using UnityEngine;

public class CutFlashEffect : MonoBehaviour
{
    public float duration = 0.5f;

    private LineRenderer _lineRenderer;
    private Color _startColor;
    private Color _endColor;
    private float _elapsed;

void Start()
{
    _lineRenderer = GetComponent<LineRenderer>();
    if (_lineRenderer == null)
    {
        Debug.LogWarning("CutFlashEffect: No LineRenderer found.");
        return;
    }

    // ✅ Voeg deze regels toe
    _lineRenderer.positionCount = 2;
    _lineRenderer.SetPosition(0, Vector3.zero);
    _lineRenderer.SetPosition(1, Vector3.up * 0.2f); // verticale lijn van 20 cm

    _startColor = _lineRenderer.startColor;
    _endColor = _lineRenderer.endColor;
    _elapsed = 0f;
}


    void Update()
    {
        if (_lineRenderer == null) return;

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
