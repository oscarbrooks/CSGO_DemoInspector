using DemoInfo;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventsHandler {
    private Parser _parser;

    private PlaybackManager _playbackManager;
    private GraphicsManager _graphicsManager;
    private UIManager _uiManager;
    private MatchInfoManager _matchInfoManager;

    public EventsHandler(Parser parser)
    {
        _parser = parser;
        _playbackManager = _parser.GetComponent<PlaybackManager>();
        _graphicsManager = _parser.GetComponent<GraphicsManager>();
        _uiManager = _parser.GetComponent<UIManager>();
        _matchInfoManager = _parser.GetComponent<MatchInfoManager>();
    }

    public void OnHeaderParsed(object sender, object e)
    {
        var eventArgs = (HeaderParsedEventArgs)e;
        var parser = (DemoParser)sender;

        _matchInfoManager.SetTickrate(parser.TickRate);

        _matchInfoManager.AddRound(parser.CTScore + parser.TScore + 1, parser.CurrentTick);

        _graphicsManager.SetMapRadar(eventArgs.Header.MapName);
    }

    public void OnRoundEnd(object sender, object e)
    {
        var parser = (DemoParser)sender;
        var roundNumber = parser.CTScore + parser.TScore;
        _uiManager.ParsingProgressLoaderUI.UpdateInfo($"{roundNumber} rounds");
    }

    public void OnRoundStart(object sender, object e)
    {
        if (!_parser.MatchStarted) return;
        var parser = (DemoParser)sender;
        var roundNumber = parser.CTScore + parser.TScore + 1;

        _matchInfoManager.AddRound(roundNumber, parser.CurrentTick);

        _playbackManager.Rounds.Add(new Round(roundNumber, parser.CurrentTick));

        if (!_playbackManager.IsInitialized) {
            var players = parser.PlayingParticipants.Select(p => new PartialPlayer(p)).ToArray();
            _playbackManager.Initialize(parser.TickRate, players);
        }
    }

    public void OnGameEnd(object sender, object p)
    {
        var players = (PartialPlayer[])p;
        var parser = (DemoParser)sender;
        //_playbackManager.Initialize(parser.TickRate, players);
    }

    public void OnTickDone(object sender, object e)
    {
        var parser = (DemoParser)sender;

        _parser.Progress = parser.ParsingProgess;
        if (!_parser.MatchStarted) return;

        var players = parser.PlayingParticipants
                            .Select(p => new PartialPlayer(p))
                            .ToArray();

        var frame = new Frame()
        {
            Tick = parser.CurrentTick,
            Players = players
        };
        _playbackManager.Frames.Add(frame);

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
            _playbackManager.AddNadeProjectileFrame(nadeProjectileFrame);
        }

    }

    public void OnWeaponFired(object sender, object e)
    {
        if (!_parser.MatchStarted) return;
        var eventArgs = (WeaponFiredEventArgs)e;
        var parser = (DemoParser)sender;

        if (_playbackManager.AddNadeFrame(parser, eventArgs)) return;

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
        if (_playbackManager.WeaponFiredFrames.Any()) frame = _playbackManager.WeaponFiredFrames.Last();

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
            _playbackManager.WeaponFiredFrames.Add(frame);
        }
        else
        {
            frame.WeaponFires.Add(weaponFire);
        }
    }
}
