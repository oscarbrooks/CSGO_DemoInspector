using UnityEngine;
using DemoInfo;

public struct PartialPlayer {
    public string SteamID { get; set; }
    public string Name { get; set; }
    public Team Team { get; set; }
    public bool IsAlive { get; set; }
    public Vector3 Position { get; set; }
    public float ViewX { get; set; }
    public float ViewY { get; set; }
    public PartialWeapon Weapon { get; set; }

    public PartialPlayer(Player player)
    {
        SteamID = player.SteamID.ToString();
        Name = player.Name;
        Team = player.Team;
        IsAlive = player.IsAlive;
        Position = new Vector3(-player.Position.X, player.Position.Z, -player.Position.Y);
        ViewX = player.ViewDirectionX;
        ViewY = player.ViewDirectionY;
        Weapon = new PartialWeapon(player.ActiveWeapon);
    }
}

public class PartialWeapon
{
    public string Name { get; set; }

    public PartialWeapon(Equipment equipment)
    {
        Name = equipment != null
            ? equipment.Weapon.ToString()
            : "NONE";
    }
}