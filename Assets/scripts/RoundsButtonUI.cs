using UnityEngine;
using UnityEngine.UI;

public class RoundsButtonUI : MonoBehaviour {
    public int RoundNumber { get; set; }

    private PlaybackManager _playbackManager;

	void Start () {
        _playbackManager = GameObject.Find("SceneManager").GetComponent<PlaybackManager>();
	}
	
	void Update () {
		
	}

    public void GoToRound()
    {
        _playbackManager.SkipToRound(RoundNumber);
    }
}
