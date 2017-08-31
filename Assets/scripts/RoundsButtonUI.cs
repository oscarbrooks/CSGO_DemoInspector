using UnityEngine;
using UnityEngine.UI;

public class RoundsButtonUI : MonoBehaviour {
    public int RoundNumber { get; set; }

	void Start () {
	}
	
	void Update () {
		
	}

    public void GoToRound()
    {
        PlaybackManager.Instance.SkipToRound(RoundNumber);
    }
}
