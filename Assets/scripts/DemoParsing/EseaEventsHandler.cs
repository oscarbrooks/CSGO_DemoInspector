using DemoInfo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EseaEventsHandler : IDemoEventHandler {
    private Parser _parser;
    private const float _tickrate = 128;

    private ValveDemoEventsHandler _defaultEventsHandler;

    public EseaEventsHandler(Parser parser)
    {
        _parser = parser;
        _defaultEventsHandler = new ValveDemoEventsHandler(_parser);
    }

    public void OnHeaderParsed(object sender, object e)
    {
        var eventArgs = (HeaderParsedEventArgs)e;
        var parser = (DemoParser)sender;

        MatchInfoManager.Instance.SetTickrate(_tickrate);

        MatchInfoManager.Instance.AddRound(parser.CTScore + parser.TScore + 1, parser.CurrentTick);

        GraphicsManager.Instance.SetMapRadar(eventArgs.Header.MapName);
    }

    public void OnRoundEnd(object sender, object e)
    {
        var parser = (DemoParser)sender;

        var roundNumber = parser.CTScore + parser.TScore;
    }

    public void OnRoundStart(object sender, object e)
    {
        if (!_parser.MatchStarted) return;

        var parser = (DemoParser)sender;

        var roundNumber = parser.CTScore + parser.TScore + 1;

        MatchInfoManager.Instance.AddRound(roundNumber, parser.CurrentTick);

        PlaybackManager.Instance.Rounds.Add(new Round(roundNumber, parser.CurrentTick));

        if (!PlaybackManager.Instance.IsInitialized)
        {
            var players = parser.PlayingParticipants.Select(p => new PartialPlayer(p)).ToArray();
            PlaybackManager.Instance.Initialize(players);
        }
    }

    public void OnPlayerTeam(object sender, object ea)
    {
        _defaultEventsHandler.OnPlayerTeam(sender, ea);
    }

    public void OnTickDone(object sender, object e)
    {
        var parser = (DemoParser)sender;

        if (!_parser.MatchStarted) return;

        var players = parser.PlayingParticipants
                            .Select(p => new PartialPlayer(p))
                            .ToArray();

        var frame = new Frame()
        {
            Tick = parser.CurrentTick,
            Players = players
        };

        PlaybackManager.Instance.Frames.Add(frame);

        if (parser.Nades.Count != 0)
        {
            var nades = parser.Nades.Select(n => n.Value.Copy())
                                    .Where(n => n.IsReady)
                                    .ToList();

            var nadeProjectileFrame = new NadeProjectileFrame()
            {
                Tick = parser.CurrentTick,
                NadeProjectiles = nades
            };

            NadePlaybackManager.Instance.AddNadeProjectileFrame(nadeProjectileFrame);
        }

    }

    public void OnWeaponFired(object sender, object e)
    {
        if (!_parser.MatchStarted) return;
        var eventArgs = (WeaponFiredEventArgs)e;
        var parser = (DemoParser)sender;

        if (NadePlaybackManager.Instance.AddNadeFrame(parser, eventArgs)) return;

        var shooter = eventArgs.Shooter;

        var weaponFire = new WeaponFire()
        {
            ShooterSteamID = shooter.SteamID.ToString(),
            Weapon = new PartialWeapon(eventArgs.Weapon),
            ShooterPosition = new Vector3(-shooter.Position.X, shooter.Position.Z, -shooter.Position.Y),
            ViewX = shooter.ViewDirectionX,
            ViewY = shooter.ViewDirectionY
        };

        WeaponFireFrame frame = null;

        if (PlaybackManager.Instance.WeaponFiredFrames.Any()) frame = PlaybackManager.Instance.WeaponFiredFrames.Last();

        if (frame == null || frame.Tick != parser.CurrentTick)
        {
            frame = new WeaponFireFrame()
            {
                Tick = parser.CurrentTick,
                WeaponFires = new List<WeaponFire>()
                {
                    weaponFire
                }
            };
            PlaybackManager.Instance.WeaponFiredFrames.Add(frame);
        }
        else
        {
            frame.WeaponFires.Add(weaponFire);
        }
    }

    public void OnPlayerHurt(object sender, object e)
    {
        if (!_parser.MatchStarted) return;
        var eventArgs = (PlayerHurtEventArgs)e;
        var parser = (DemoParser)sender;

        var playerHurt = new PlayerHurt()
        {
            Victim = new PartialPlayer(eventArgs.Player),
            Weapon = new PartialWeapon(eventArgs.Weapon),
            HitGroup = eventArgs.Hitgroup
        };

        if (eventArgs.Attacker != null) playerHurt.Attacker = new PartialPlayer(eventArgs.Attacker);
        else playerHurt.IsFallDamage = true;

        PlayerHurtFrame frame = null;

        if (PlaybackManager.Instance.PlayerHurtFrames.Any()) frame = PlaybackManager.Instance.PlayerHurtFrames.Last();

        if (frame == null || frame.Tick != parser.CurrentTick)
        {
            frame = new PlayerHurtFrame()
            {
                Tick = parser.CurrentTick,
                PlayerHurts = new List<PlayerHurt>()
                    {
                        playerHurt
                    }
            };

            PlaybackManager.Instance.PlayerHurtFrames.Add(frame);
        }
        else
        {
            frame.PlayerHurts.Add(playerHurt);
        }
    }

    public void OnSmokeNadeStarted(object sender, object ea)
    {
        _defaultEventsHandler.OnSmokeNadeStarted(sender, ea);
    }
}
