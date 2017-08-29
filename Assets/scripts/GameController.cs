using UnityEngine;

public class GameController : MonoBehaviour {

    private PlaybackManager _playbackManager;

	void Start () {
        //_playbackManager = GetComponent<PlaybackManager>();
	}
	
	void Update () {
        if (Input.GetButton("Jump")) Time.timeScale = 0.1f;
        else Time.timeScale = 1;
	}
}
