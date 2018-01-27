using System.Collections;
using UnityEngine;

public class WeaponFireGraphics : MonoBehaviour {

    public float defaultLength = 15;

    public Vector3 Direction { get; set; }
    public Vector3 StartPoint { get; set; }
    public Vector3 EndPoint { get; set; }
    public bool HasEndPoint { get; set; }

    public Color EmissionColor { get; set; } = new Color(1, 0.9782962f, 0.2132353f);

    private LineRenderer _lineRenderer;

    private void Start () {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;

        _lineRenderer.materials[0].SetColor("_EmissionColor", EmissionColor);

        SetPositions();

        var fadeCoroutine = Fade(0.3f);
        StartCoroutine(fadeCoroutine);

        Destroy(gameObject, 1.0f);
    }
	
	private void Update () {

    }

    private void SetPositions()
    {
        var endPoint = HasEndPoint
            ? EndPoint
            : StartPoint + Vector3.Normalize(Direction) * defaultLength;

        _lineRenderer.SetPositions(new Vector3[2] {
            StartPoint + PlayerGraphics.PlayerRootOffset,
            endPoint + PlayerGraphics.PlayerRootOffset });
    }

    private IEnumerator Fade(float seconds)
    {
        var startWidth = _lineRenderer.endWidth;

        for (float f = seconds; f >= 0; f -= Time.deltaTime)
        {
            var color = _lineRenderer.materials[0].color;

            color.a = color.a * (f / seconds);

            _lineRenderer.materials[0].color = color;

            var emissionColor = _lineRenderer.materials[0].GetColor("_EmissionColor");

            _lineRenderer.materials[0].SetColor("_EmissionColor", emissionColor * (f / seconds));

            _lineRenderer.endWidth = startWidth * (f / seconds);

            yield return null;
        }
    }
}
