using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SpectateController : MonoBehaviour {

    public SpectateMode SpectateMode;

    public PlayerGraphics CurrentPlayer;

    private Vector3 _headOffset = new Vector3(0, 0.7f, 0);

    [SerializeField]
    private GameObject _crosshair;

	private void Start () {
		
	}
	
	private void Update () {
        if (SpectateMode == SpectateMode.POV) SpectatePov();
	}

    public void FreeSpectate()
    {
        if (CurrentPlayer != null) CurrentPlayer.Hide(false);

        SpectateMode = SpectateMode.Free;

        _crosshair.SetActive(false);
    }

    public void SetPlayer(PlayerGraphics playerGraphics)
    {
        if (CurrentPlayer != null) CurrentPlayer.Hide(false);

        CurrentPlayer = playerGraphics;

        CurrentPlayer.Hide(true);

        SpectateMode = SpectateMode.POV;

        _crosshair.SetActive(true);
    }

    private void SpectatePov()
    {
        if (CurrentPlayer == null) return;

        transform.position = Vector3.Lerp(transform.position, CurrentPlayer.transform.position + _headOffset, Time.deltaTime * 64);

        transform.LookAt(transform.position + CurrentPlayer.TargetViewDirection);
    }

}

public enum SpectateMode
{
    Free,
    POV,
    Orbit
}
