using UnityEngine;

public class NadeProjectileGraphics : MonoBehaviour {
    public float Tickrate { get; set; } = 64;
    public float Smoothing = 1.5f;

    public Vector3 _targetPosition;
    public Vector3 Offset { get; set; } = Vector3.zero;

    private void Start () {
		
	}
	
	private void Update () {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * Tickrate / Smoothing);
    }

    public void UpdatePosition(Vector3 position)
    {
        position += Offset;
        position.y = Mathf.Clamp(position.y, 0, 100);
        _targetPosition = position;
    }
}
