using DemoInfo;
using UnityEngine;

public class NadeProjectileGraphics : MonoBehaviour {
    public EquipmentElement NadeType { get; set; }
    public float Tickrate { get; set; } = 64;
    public float Smoothing = 1.5f;

    public Vector3 _targetPosition;
    public Vector3 Offset { get; set; } = Vector3.zero;

    public GameObject Graphics;
    private INadeMechanics _nadeMechanics;

    private void Start () {
        InitializeNadeMechanics();
	}
	
	private void Update () {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * Tickrate / Smoothing);

        _nadeMechanics.Update();
    }

    public void UpdatePosition(Vector3 position)
    {
        position += Offset;

        if (GraphicsManager.Instance.PlaybackMode == PlaybackMode.TwoD) position.y = Mathf.Clamp(position.y, 0, 100);

        _nadeMechanics.UpdateVelocity(position - _targetPosition);

        _targetPosition = position;
    }

    private void InitializeNadeMechanics()
    {
        switch (NadeType)
        {
            case EquipmentElement.Smoke:
                _nadeMechanics = new SmokeNadeMechanics();
                break;
            default:
                break;
        }

        _nadeMechanics.Initialize(this);
    }
}
