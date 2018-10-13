using DemoInfo;
using UnityEngine;

public class NadeProjectileGraphics : MonoBehaviour {
    public EquipmentElement NadeType { get; set; }
    public float Tickrate { get; set; } = 64;
    public float Smoothing = 1.5f;

    public Vector3 _targetPosition;
    public Vector3 Offset { get; set; } = Vector3.zero;

    private INadeMechanics _nadeMechanics;

    [SerializeField]
    private TrailRendererFollower _trailPrefab;
    public TrailRendererFollower Trail;

    private bool _isInitialized = false;

    private const string _nadesFolder = "prefabs/utility/";

    private void Start () {
	}
	
	private void Update () {
        EnsureInitialized();

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * Tickrate / Smoothing);

        _nadeMechanics.Update();
    }

    public void UpdatePosition(Vector3 position)
    {
        EnsureInitialized();

        position += Offset;

        if (GraphicsManager.Instance.PlaybackMode == PlaybackMode.TwoD) position.y = Mathf.Clamp(position.y, 0, 100);

        _nadeMechanics.UpdateVelocity(position - _targetPosition);

        _targetPosition = position;
    }

    public TrailRendererFollower CreateTrail()
    {
        Trail = Instantiate(_trailPrefab).GetComponent<TrailRendererFollower>();
        Trail.FollowTarget = transform;

        return Trail;
    }

    private void InitializeNadeMechanics()
    {
        switch (NadeType)
        {
            case EquipmentElement.Smoke:
                
                _nadeMechanics = new SmokeNadeMechanics(this, InstantiateGraphics("smoke_grenade"));
                break;
            default:
                _nadeMechanics = new DefaultNadeMechanics(this, InstantiateGraphics("flashbang"));
                break;
        }

        _isInitialized = true;
    }

    private GameObject InstantiateGraphics(string name)
    {
        return Instantiate(Resources.Load<GameObject>(_nadesFolder + name), transform);
    }

    private void EnsureInitialized()
    {
        if (!_isInitialized) InitializeNadeMechanics();
    }
}
