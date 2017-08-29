using UnityEngine;

public class PlayerGraphics : MonoBehaviour {
    public float Tickrate { get; set; } = 64;

    private Vector3 _targetPosition;
    private Vector3 _targetViewDirection;

    void Start () {
		
	}
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * Tickrate);
        Debug.DrawLine(transform.position, transform.position + _targetViewDirection * 5, Color.red);
        transform.LookAt(transform.position + _targetViewDirection);
	}

    public void UpdatePosition(Vector3 position)
    {
        _targetPosition = position;
        _targetPosition.y = 1.5f;
    }

    public void UpdateViewAngle(float viewX, float viewY)
    {
        _targetViewDirection = DemoInfoHelper.ViewAnglesToVector3(viewX, viewY);
        _targetViewDirection.y = 0;
    }

    public bool IsAlive(bool isAlive)
    {
        if(gameObject.activeSelf != isAlive) gameObject.SetActive(isAlive);
        return isAlive;
    }
}
