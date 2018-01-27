using System.Collections.Generic;
using UnityEngine;
using DemoInfo;
using System;

public interface IFrame
{
    int Tick { get; set; }
}

public struct Frame : IFrame {
    public int Tick { get; set; }
    public PartialPlayer[] Players { get; set; }
}

public class WeaponFireFrame : IFrame
{
    public int Tick { get; set; }
    public List<WeaponFire> WeaponFires { get; set; }
}

public class NadeThrowFrame : IFrame
{
    public int Tick { get; set; }
    public int Round { get; set; }
    public List<NadeThrow> NadeThrows { get; set; }
}

public class NadeProjectileFrame : IFrame
{
    public int Tick { get; set; }
    public List<NadeProjectile> NadeProjectiles { get; set; }
}

public class PlayerHurtFrame : IFrame
{
    public int Tick { get; set; }
    public List<PlayerHurt> PlayerHurts { get; set; }
}

public struct WeaponFire
{
    public string ShooterSteamID { get; set; }
    public PartialWeapon Weapon { get; set; }
    public Vector3 ShooterPosition { get; set; }
    public float ViewX { get; set; }
    public float ViewY { get; set; }
}

public struct PlayerHurt
{
    public PartialPlayer Victim { get; set; }
    public PartialPlayer Attacker { get; set; }
    public PartialWeapon Weapon { get; set; }
    public Hitgroup HitGroup { get; set; }
    public bool IsFallDamage { get; set; }
}

public struct NadeThrow
{
    public string Thrower { get; set; }
    public EquipmentElement NadeType { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Direction { get; set; }
}
