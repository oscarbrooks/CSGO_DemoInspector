using UnityEngine;
using UnityEngine.UI;

public class PlaybackControlsUI : MonoBehaviour {

    private PlayButton _playButton { get; set;}

	void Start () {
        _playButton = new PlayButton(transform);
        ToggleUIPlayPause(PlaybackManager.Instance.IsPlaying);
	}
	
	void Update () {
		
	}

    public void TogglePlayPause()
    {
        PlaybackManager.Instance.TogglePlayPause();
        ToggleUIPlayPause(PlaybackManager.Instance.IsPlaying);
    }

    public void SkipForwardBackward(int seconds)
    {
        PlaybackManager.Instance.Skip(seconds);
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