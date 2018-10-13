using UnityEngine;
using DemoInfo;

public class PlayerHurtGraphicsManager : SingletonMonoBehaviour<PlayerHurtGraphicsManager> {

	private void Start () {
		
	}
	
	private void Update () {
		
	}

    public void ResetSelf() { }

    public void UpdatePlayerHurtFrame(PlayerHurtFrame frame, int playbackScale, Vector3 mapOffset)
    {
        for (int i = 0; i < frame.PlayerHurts.Count; i++)
        {
            var playerHurt = frame.PlayerHurts[i];

            if(!playerHurt.IsFallDamage)
            {
                CreateWeaponFireGraphics(playerHurt, playbackScale, mapOffset);
            }

            //damage victim
        }
    }

    private WeaponFireGraphics CreateWeaponFireGraphics(PlayerHurt playerHurt, int playbackScale, Vector3 mapOffset)
    {
        var weaponFireClone = Instantiate(WeaponGraphicsManager.Instance.WeaponFirePrefab);

        var weaponGraphics = weaponFireClone.GetComponent<WeaponFireGraphics>();

        var startPoint = (playerHurt.Attacker.Position / playbackScale) - mapOffset + PlayersManager.HeadOffset;

        weaponGraphics.HasEndPoint = true;

        var endPoint = (playerHurt.Victim.Position / playbackScale) - mapOffset;

        if (playerHurt.HitGroup == Hitgroup.Head) endPoint += PlayersManager.HeadOffset;

        if (GraphicsManager.Instance.PlaybackMode == PlaybackMode.TwoD) startPoint.y = endPoint.y = 0.3f;

        weaponGraphics.StartPoint = startPoint;

        weaponGraphics.EndPoint = endPoint;

        weaponGraphics.EmissionColor = new Color(1, 0.2946248f, 0.2132353f);

        return weaponGraphics;
    }
}
