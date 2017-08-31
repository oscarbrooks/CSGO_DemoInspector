using UnityEngine;

public class GameController : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
        if (Input.GetButton("Jump")) Time.timeScale = 0.1f;
        else Time.timeScale = 1;
	}
}
