using UnityEngine;

public class WeaponFireGraphics : MonoBehaviour {

    public Vector3 Direction { get; set; }

	void Start () {
		
	}
	
	void Update () {
        transform.position += new Vector3(Direction.x, 0, Direction.z) * 4;
	}
}
