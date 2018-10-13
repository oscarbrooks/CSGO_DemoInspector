using System;
using UnityEngine;
using DemoInfo;

public struct PartialPlayer {
    public string SteamID { get; set; }
    public string Name { get; set; }
    public Team Team { get; set; }
    public string Clantag { get; set; }
    public bool IsBot { get; set; }
    public bool IsAlive { get; set; }
    public int Health { get; set; }
    public int Money { get; set; }
    public Vector3 Position { get; set; }
    public float ViewX { get; set; }
    public float ViewY { get; set; }
    public PartialWeapon Weapon { get; set; }

    public PartialPlayer(Player player)
    {
        SteamID = player.SteamID.ToString();
        Name = player.Name;
        Team = player.Team;
        Clantag = player.AdditionaInformations.Clantag;
        IsBot = player.IsBot;
        IsAlive = player.IsAlive;
        Health = player.HP;
        Money = player.Money;

        Position = new Vector3(-player.Position.X, player.Position.Z, -player.Position.Y);
        ViewX = player.ViewDirectionX;
        ViewY = player.ViewDirectionY;
        Weapon = new PartialWeapon(player.ActiveWeapon);
    }
}

[System.Serializable]
public class PartialWeapon
{
    public string Name { get; set; }
    public string SkinId { get; set; }
    public EquipmentClass EquipmentClass { get; set; }

    public PartialWeapon(Equipment equipment)
    {
        Name = equipment != null
            ? equipment.Weapon.ToString()
            : "NONE";

        if (equipment == null) return;

        SkinId = equipment.OriginalString;

        EquipmentClass = (EquipmentClass) Enum.Parse(typeof(EquipmentClass), equipment.Class.ToString());
    }
}