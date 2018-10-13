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
    public Vector3 TargetViewDirection;

    public Transform NameTag;
    private TextMesh _nameText;

    private Camera Cam;

    public WeaponHolder WeaponHolder;

    private void Start () {
        Cam = Camera.main;

        NameTag = transform.Find("NameTag");

        _nameText = NameTag.GetComponent<TextMesh>();

        WeaponHolder = GetComponentInChildren<WeaponHolder>();
	}
	
	private void Update () {
        //Debug.Log($"{_nameText.text} - {_targetPosition} | tickrate: {Tickrate}");

        var lerpPos = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * Tickrate);

        if (!VectorIsNaN(lerpPos)) transform.position = lerpPos;
        else transform.position = _targetPosition;

        Debug.DrawLine(transform.position, transform.position + TargetViewDirection * 5, Color.red);

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
        TargetViewDirection = DemoInfoHelper.ViewAnglesToVector3(viewX, viewY);
    }

    private void UpdatePlayerAim()
    {
        var bodyLookDirection = TargetViewDirection;

        bodyLookDirection.y = 0;

        transform.LookAt(transform.position + bodyLookDirection);

        if (GraphicsManager.Instance.PlaybackMode == PlaybackMode.TwoD) return;

        var pitchAngle = Vector3.Angle(new Vector3(TargetViewDirection.x, 0, TargetViewDirection.z), TargetViewDirection);
        
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

    public void ShowHead(bool shouldShowHead)
    {
        if (_ctGraphicsSource == null) _ctGraphicsSource = CTModel.GetComponent<PlayerGraphicsSource>();
        if (_tGraphicsSource == null) _tGraphicsSource = TModel.GetComponent<PlayerGraphicsSource>();

        _ctGraphicsSource.Head.gameObject.SetActive(shouldShowHead);
        _tGraphicsSource.Head.gameObject.SetActive(shouldShowHead);
    }

    public void Hide(bool shouldHide)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = !shouldHide;
        }
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

    private bool VectorIsNaN(Vector3 vect)
    {
        return float.IsNaN(vect.x)
            || float.IsNaN(vect.y)
            || float.IsNaN(vect.z);
    }
}
