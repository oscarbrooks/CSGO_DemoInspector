using System;
using UnityEngine;

public class PlaybackModeUI : MonoBehaviour {
    
	private void Start () {
		
	}
	
	private void Update () {
		
	}

    public void SwitchPlaybackMode(string mode)
    {
        var playbackMode = (PlaybackMode) Enum.Parse(typeof(PlaybackMode), mode);
        GraphicsManager.Instance.SwitchToPlaybackMode(playbackMode);
    }
}
