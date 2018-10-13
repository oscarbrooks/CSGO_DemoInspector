using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DemoInfo;
using System;

public class NadePlaybackManager : SingletonMonoBehaviour<NadePlaybackManager>
{
    public List<NadeThrowFrame> NadeThrowFrames { get; set; } = new List<NadeThrowFrame>();
    public List<NadeProjectileFrame> NadeProjectileFrames { get; set; } = new List<NadeProjectileFrame>();
    public List<NadeEffectFrame> NadeEffectFrames { get; set; } = new List<NadeEffectFrame>();

    public bool AddNadeFrame(DemoParser parser, WeaponFiredEventArgs eventArgs)
    {
        if (eventArgs.Weapon.Class != EquipmentClass.Grenade) return false;

        var nadeThrow = new NadeThrow()
        {
            NadeType = eventArgs.Weapon.Weapon,
            Thrower = eventArgs.Shooter.SteamID.ToString(),
            Position = new Vector3(-eventArgs.Shooter.Position.X, eventArgs.Shooter.Position.Z, -eventArgs.Shooter.Position.Y),
            Direction = DemoInfoHelper.ViewAnglesToVector3(eventArgs.Shooter.ViewDirectionX, eventArgs.Shooter.ViewDirectionY)
        };

        NadeThrowFrame frame = null;
        if (NadeThrowFrames.Count != 0) frame = NadeThrowFrames.Last();
        if (frame == null || frame.Tick != parser.CurrentTick)
        {
            frame = new NadeThrowFrame()
            {
                Tick = parser.CurrentTick,
                Round = MatchInfoManager.Instance.Rounds.Last().Number,
                NadeThrows = new List<NadeThrow>()
                {
                    nadeThrow
                }
            };
            NadeThrowFrames.Add(frame);
        }
        else
        {
            frame.NadeThrows.Add(nadeThrow);
        }

        return true;
    }

    public void AddNadeProjectileFrame(NadeProjectileFrame frame)
    {
        NadeProjectileFrames.Add(frame);
    }

    public void AddSmokeNadeEffect(DemoParser parser, SmokeEventArgs eventArgs)
    {
        if (!NadeEffectFrames.Any(n => n.Tick == parser.CurrentTick)) NadeEffectFrames.Add(new NadeEffectFrame() {
            Tick = parser.CurrentTick,
            Round = MatchInfoManager.Instance.Rounds.Last().Number,
            NadeEffects = new List<NadeEffect>()
        });

        if (eventArgs.ThrownBy == null) return;

        var nadeEffect = new NadeEffect() {
            Guid = Guid.NewGuid(),
            Duration = 18,
            Thrower = eventArgs.ThrownBy.SteamID.ToString(),
            NadeType = EquipmentElement.Smoke,
            Position = (DemoInfoHelper.SourceToUnityVector(eventArgs.Position.Copy()) / GraphicsManager.PlaybackScale) - GraphicsManager.Instance.Map.Offset
        };

        NadeEffectFrames.Last().NadeEffects.Add(nadeEffect);
    }
}
