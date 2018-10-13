using DemoInfo;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class PlayersManager : SingletonMonoBehaviour<PlayersManager> {

    public static Vector3 HeadOffset { get; private set; }  = new Vector3(0, 0.5f, 0);

    public GameObject PlayerModelPrefab;
    public GameObject CTPlayerPrefab;
    public GameObject TPlayerPrefab;

    private Dictionary<string, PlayerGraphics> _players = new Dictionary<string, PlayerGraphics>();

    [SerializeField]
    private PlayerSideInfoController _playerSideInfoController;

    private void Start () {
		
	}
	
	private void Update () {
		
	}

    public void ResetSelf()
    {
        _players = new Dictionary<string, PlayerGraphics>();
    }

    public PlayerGraphics GetBySteamId(string steamId)
    {
        PlayerGraphics playerGraphics = null;

        _players.TryGetValue(steamId, out playerGraphics);

        return playerGraphics;
    }

    public void CreatePlayers(PartialPlayer[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];

            if (_players.ContainsKey(player.SteamID.ToString())) continue;

            var playerGraphics = CreatePlayerGraphics(player);

            _playerSideInfoController.AddPlayer(player);

            _players.Add(player.SteamID.ToString(), playerGraphics);
        }
    }

    private PlayerGraphics CreatePlayerGraphics(PartialPlayer player)
    {
        var playerModelClone = Instantiate(PlayerModelPrefab);
        var playerGraphics = playerModelClone.GetComponent<PlayerGraphics>();

        playerGraphics.CTModel = Instantiate(CTPlayerPrefab);
        playerGraphics.TModel = Instantiate(TPlayerPrefab);

        playerGraphics.CTModel.transform.parent = playerGraphics.TModel.transform.parent = playerModelClone.transform;

        if (player.Team == Team.CounterTerrorist) playerGraphics.TModel.SetActive(false);
        else playerGraphics.CTModel.SetActive(false);

        playerModelClone.name = "Player " + player.SteamID;

        playerGraphics.Tickrate = GraphicsManager.Instance.Tickrate;
        playerGraphics.NameTag.GetComponent<TextMesh>().text = player.Name;

        return playerGraphics;
    }

    public void UpdatePlayers(PartialPlayer[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];

            PlayerGraphics playerGraphics;

            if (_players.TryGetValue(player.SteamID.ToString(), out playerGraphics))
            {
                if (!playerGraphics.IsAlive(player.IsAlive)) continue;

                var pos = new Vector3(player.Position.x, player.Position.y, player.Position.z) / GraphicsManager.PlaybackScale;

                playerGraphics.UpdatePosition(pos - GraphicsManager.Instance.Map.Offset);

                playerGraphics.UpdateViewAngle(player.ViewX, player.ViewY);

                playerGraphics.EnsureCorrectPlayerModel(player);

                playerGraphics.WeaponHolder.SetWeapon(player.Weapon);
            }
        }
    }
}
