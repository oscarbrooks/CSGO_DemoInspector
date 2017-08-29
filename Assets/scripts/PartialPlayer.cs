using UnityEngine;
using DemoInfo;

public struct PartialPlayer {
    public string SteamID { get; set; }
    public Team Team { get; set; }
    public bool IsAlive { get; set; }
    public Vector3 Position { get; set; }
    public float ViewX { get; set; }
    public float ViewY { get; set; }

    public PartialPlayer(Player player)
    {
        SteamID = player.SteamID.ToString();
        Team = player.Team;
        IsAlive = player.IsAlive;
        Position = new Vector3(-player.Position.X, player.Position.Z, -player.Position.Y);
        ViewX = player.ViewDirectionX;
        ViewY = player.ViewDirectionY;
    }
}

public struct PartialWeapon
{
    public string Name { get; set; }

    public PartialWeapon(Equipment equipment)
    {
        Name = equipment.Weapon.ToString();
    }
}