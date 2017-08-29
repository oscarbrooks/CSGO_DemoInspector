using DemoInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphicsManager : MonoBehaviour {

    public GameObject CTPlayerPrefab;
    public GameObject TPlayerPrefab;
    public GameObject WeaponFirePrefab;
    public GameObject NadePrefab;
    public GameObject NadeProjectilePrefab;

    public float Tickrate { get; set; } = 64;

    private const int _playbackScale = 35;

    private Dictionary<string, PlayerGraphics> _players = new Dictionary<string, PlayerGraphics>();
    private Dictionary<Guid, NadeProjectileGraphics> _nadeProjectiles = new Dictionary<Guid, NadeProjectileGraphics>();

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void SetMapRadar(string name)
    {
        var mapOverviews = GameObject.Find("GameContent").GetComponent<MapOverviews>();
        if (mapOverviews.UseSimpleRadar) name += "_sr";
        var map = mapOverviews.Maps.First(m => m.Name == name);
        var mapPlane = GameObject.Find("MapOverviewRadar");
        mapPlane.GetComponent<Renderer>().material.mainTexture = map.Texture;
        mapPlane.transform.position = map.Offset;
        mapPlane.transform.localScale = map.Scale;
    }

    public void CreatePlayers(PartialPlayer[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];

            GameObject playerClone;
            if (player.Team == Team.CounterTerrorist) playerClone = Instantiate(CTPlayerPrefab);
            else playerClone = Instantiate(TPlayerPrefab);

            playerClone.name = player.SteamID;
            var playerGraphics = playerClone.GetComponent<PlayerGraphics>();
            playerGraphics.Tickrate = Tickrate;
            _players.Add(player.SteamID.ToString(), playerGraphics);
        }
    }

    public void UpdatePlayers(PartialPlayer[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];
            PlayerGraphics playerGraphics;
            if(_players.TryGetValue(player.SteamID.ToString(), out playerGraphics))
            {
                if (!playerGraphics.IsAlive(player.IsAlive)) continue;
                var pos = new Vector3(player.Position.x, player.Position.y, player.Position.z) / _playbackScale;
                playerGraphics.UpdatePosition(pos);
                playerGraphics.UpdateViewAngle(player.ViewX, player.ViewY);
            }
        }
    }

    public void DisplayWeaponFireFrame(WeaponFireFrame frame)
    {
        for (int i = 0; i < frame.WeaponFires.Count; i++)
        {
            var weaponFire = frame.WeaponFires[i];
            var weaponFireClone = Instantiate(WeaponFirePrefab);
            var weaponGraphics = weaponFireClone.GetComponent<WeaponFireGraphics>();
            weaponGraphics.Direction = DemoInfoHelper.ViewAnglesToVector3(weaponFire.ViewX, weaponFire.ViewY);
            var pos = weaponFire.ShooterPosition / _playbackScale;
            pos.y = 1.7f;
            weaponFireClone.transform.position = pos;
            Destroy(weaponFireClone, 1);
        }
    }

    public void DisplayNadeFrame(NadeThrowFrame frame)
    {
        for (int i = 0; i < frame.NadeThrows.Count; i++)
        {
            var nadeThrow = frame.NadeThrows[i];
            var nadeClone = Instantiate(NadePrefab);
            nadeClone.GetComponent<NadeGraphics>().NadeThrow = nadeThrow;
            var pos = nadeThrow.Position / _playbackScale;
            pos.y = 3;
            nadeClone.transform.position = pos;
            nadeClone.name = nadeThrow.NadeType.ToString() + " - " + nadeThrow.Thrower;

            Destroy(nadeClone, 5);
        }
    }

    public void UpdateNadeProjectileFrame(NadeProjectileFrame frame)
    {
        var leftOverGuids = _nadeProjectiles.Keys.ToList();
        for (int i = 0; i < frame.NadeProjectiles.Count; i++)
        {
            var nade = frame.NadeProjectiles[i];
            if (leftOverGuids.Contains(nade.Guid))
            {
                leftOverGuids.Remove(nade.Guid);
            }
            else
            {
                var pos = DemoInfoHelper.SourceToUnityVector(nade.GetPos()) / _playbackScale;
                var offset = new Vector3(0, -pos.y, 0);
                var nadeProjectileClone = Instantiate(NadeProjectilePrefab);
                nadeProjectileClone.transform.position = pos + offset;
                var nadeGraphics = nadeProjectileClone.GetComponent<NadeProjectileGraphics>();
                nadeGraphics.Offset = offset;
                nadeGraphics.Tickrate = Tickrate;
                _nadeProjectiles.Add(nade.Guid, nadeGraphics);
            }
            _nadeProjectiles[nade.Guid].UpdatePosition(DemoInfoHelper.SourceToUnityVector(nade.GetPos()) / _playbackScale);
        }

        foreach (Guid guid in leftOverGuids)
        {
            var nadeGraphics = _nadeProjectiles[guid];
            _nadeProjectiles.Remove(guid);
            Destroy(nadeGraphics.gameObject);
        }

    }

}
