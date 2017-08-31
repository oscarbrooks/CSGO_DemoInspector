using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 _orbitTarget = Vector3.zero;
    private float _sensitivity = 4;
    private float _zoomSensitivity = 10;

    private void Start () {
	}

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10);
    }

    private void LateUpdate () {
        if(Input.GetMouseButton(2))
        {
            transform.RotateAround(_orbitTarget, Vector3.up, Input.GetAxisRaw("Mouse X") * _sensitivity);
            transform.RotateAround(_orbitTarget, transform.right, Input.GetAxisRaw("Mouse Y") * -_sensitivity);
        }

        HandleZoom();

        HandleMove();

        transform.LookAt(_orbitTarget);
	}

    private void HandleZoom()
    {
        var zoomInput = Input.GetAxisRaw("Mouse ScrollWheel");

        if (zoomInput == 0) return;

        var direction = zoomInput > 0 ? 1 : -1;

        transform.position = transform.position + transform.forward * direction * _zoomSensitivity;
        Debug.DrawLine(transform.position, transform.position + (transform.forward * _zoomSensitivity), Color.red);
    }

    private void HandleMove()
    {
        var moveVect = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));

        moveVect.y = 0;

        transform.position += moveVect;

        _orbitTarget += moveVect;
    }
}
