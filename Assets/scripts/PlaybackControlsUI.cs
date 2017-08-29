using UnityEngine;
using UnityEngine.UI;

public class PlaybackControlsUI : MonoBehaviour {

    private PlayButton _playButton { get; set;}
    private PlaybackManager _playbackManager { get; set; }

	void Start () {
        _playButton = new PlayButton(transform);
        _playbackManager = GameObject.Find("SceneManager").GetComponent<PlaybackManager>();
        ToggleUIPlayPause(_playbackManager.IsPlaying);
	}
	
	void Update () {
		
	}

    public void TogglePlayPause()
    {
        _playbackManager.TogglePlayPause();
        ToggleUIPlayPause(_playbackManager.IsPlaying);
    }

    public void SkipForwardBackward(int seconds)
    {
        _playbackManager.Skip(seconds);
    }

    private void ToggleUIPlayPause(bool isPlaying)
    {
        _playButton.PlayIcon.SetActive(!isPlaying);
        _playButton.PauseIcon.SetActive(isPlaying);
    }
}

internal struct PlayButton
{
    public GameObject Button { get; set; }
    public GameObject PlayIcon { get; set; }
    public GameObject PauseIcon { get; set; }

    public PlayButton(Transform transform)
    {
        Button = transform.Find("PlayButton").gameObject;
        PlayIcon = Button.transform.Find("PlayIcon").gameObject;
        PauseIcon = Button.transform.Find("PauseIcon").gameObject;
    }
}