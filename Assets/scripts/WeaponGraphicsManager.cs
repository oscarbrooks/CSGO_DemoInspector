using UnityEngine;

public class WeaponGraphicsManager : SingletonMonoBehaviour<WeaponGraphicsManager> {

    public GameObject WeaponFirePrefab;

    private void Start () {
		
	}
	
	private void Update () {
		
	}

    public WeaponFireGraphics CreateWeaponFireGraphics(WeaponFire weaponFire, Vector3 position, PlaybackMode playbackMode)
    {
        var weaponFireClone = Instantiate(WeaponFirePrefab);

        var weaponGraphics = weaponFireClone.GetComponent<WeaponFireGraphics>();

        weaponGraphics.Direction = playbackMode == PlaybackMode.ThreeD
            ? DemoInfoHelper.ViewAnglesToVector3(weaponFire.ViewX, weaponFire.ViewY)
            : DemoInfoHelper.ViewAnglesToVector3(weaponFire.ViewX, 0);

        weaponGraphics.HasEndPoint = false;

        position += PlayersManager.HeadOffset;

        if (playbackMode == PlaybackMode.TwoD) position.y = 0.3f;

        weaponFireClone.transform.position = weaponGraphics.StartPoint = position;

        return weaponGraphics;
    }
}
