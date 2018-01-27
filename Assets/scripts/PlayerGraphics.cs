using DemoInfo;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour {

    public float Tickrate { get; set; } = 64;

    public static Vector3 PlayerRootOffset = new Vector3(0, 1.4f, 0);

    public GameObject CTModel { get; set; }
    public GameObject TModel { get; set; }

    private PlayerGraphicsSource _currentPlayerGraphicsSource;
    private PlayerGraphicsSource _ctGraphicsSource;
    private PlayerGraphicsSource _tGraphicsSource;

    private Vector3 _targetPosition;
    private Vector3 _targetViewDirection;

    public Transform NameTag;
    private Camera Cam;

    public WeaponHolder WeaponHolder;

    void Start () {
        Cam = Camera.main;

        NameTag = transform.Find("NameTag");

        WeaponHolder = GetComponentInChildren<WeaponHolder>();
	}
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * Tickrate);

        Debug.DrawLine(transform.position, transform.position + _targetViewDirection * 5, Color.red);

        UpdatePlayerAim();

        NameTag.LookAt(2 * transform.position - Cam.transform.position);
	}

    public void UpdatePosition(Vector3 position)
    {
        _targetPosition = position + PlayerRootOffset;

        if (GraphicsManager.Instance.PlaybackMode == PlaybackMode.TwoD) _targetPosition.y = 1.5f;
    }

    public void UpdateViewAngle(float viewX, float viewY)
    {
        _targetViewDirection = DemoInfoHelper.ViewAnglesToVector3(viewX, viewY);
    }

    private void UpdatePlayerAim()
    {
        var bodyLookDirection = _targetViewDirection;

        bodyLookDirection.y = 0;

        transform.LookAt(transform.position + bodyLookDirection);

        if (GraphicsManager.Instance.PlaybackMode == PlaybackMode.TwoD) return;

        var pitchAngle = Vector3.Angle(new Vector3(_targetViewDirection.x, 0, _targetViewDirection.z), _targetViewDirection);
        
        if(_currentPlayerGraphicsSource != null)
        {
            var headAngles = _currentPlayerGraphicsSource.Head.transform.localEulerAngles;

            headAngles.x = pitchAngle;

            _currentPlayerGraphicsSource.Head.transform.localEulerAngles = headAngles;
        }
        
        var weaponAngles = WeaponHolder.transform.localEulerAngles;

        weaponAngles.x = pitchAngle;

        WeaponHolder.transform.localEulerAngles = weaponAngles;
    }

    public bool IsAlive(bool isAlive)
    {
        if(gameObject.activeSelf != isAlive) gameObject.SetActive(isAlive);
        return isAlive;
    }

    public void EnsureCorrectPlayerModel(PartialPlayer player)
    {
        if(player.Team == Team.CounterTerrorist)
        {
            CTModel.SetActive(true);
            TModel.SetActive(false);
            if(_ctGraphicsSource == null) _ctGraphicsSource = CTModel.GetComponent<PlayerGraphicsSource>();
            _currentPlayerGraphicsSource = _ctGraphicsSource;
        }
        else
        {
            CTModel.SetActive(false);
            TModel.SetActive(true);
            if (_tGraphicsSource == null) _tGraphicsSource = TModel.GetComponent<PlayerGraphicsSource>();
            _currentPlayerGraphicsSource = _tGraphicsSource;
        }
    }
}
