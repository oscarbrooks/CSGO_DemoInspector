using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoInfo;
using System.Linq;

public class FaceItEventsHandler : IDemoEventHandler
{
    private readonly IDemoEventHandler _baseEventsHandler;

    private Parser _parser;
    private float _tickrate = 128;

    public FaceItEventsHandler(Parser parser)
    {
        _parser = parser;
        _baseEventsHandler = new ValveDemoEventsHandler(_parser);
    }

    public void OnHeaderParsed(object sender, object ea)
    {
        var eventArgs = (HeaderParsedEventArgs)ea;
        var parser = (DemoParser)sender;

        MatchInfoManager.Instance.SetTickrate(_tickrate);

        MatchInfoManager.Instance.AddRound(parser.CTScore + parser.TScore + 1, parser.CurrentTick);

        GraphicsManager.Instance.SetMapRadar(eventArgs.Header.MapName);
    }

    public void OnPlayerHurt(object sender, object ea)
    {
        _baseEventsHandler.OnPlayerHurt(sender, ea);
    }

    public void OnPlayerTeam(object sender, object ea)
    {
        _baseEventsHandler.OnPlayerTeam(sender, ea);
    }

    public void OnRoundStart(object sender, object ea)
    {
        var parser = (DemoParser)sender;

        var roundNumber = parser.CTScore + parser.TScore;

        MatchInfoManager.Instance.AddRound(roundNumber, parser.CurrentTick);

        PlaybackManager.Instance.Rounds.Add(new Round(roundNumber, parser.CurrentTick));

        if (!PlaybackManager.Instance.IsInitialized)
        {
            var players = parser.PlayingParticipants.Select(p => new PartialPlayer(p)).ToArray();
            PlaybackManager.Instance.Initialize(players);
        }
    }

    public void OnRoundEnd(object sender, object ea)
    {
        //Do nothing for now
    }

    public void OnTickDone(object sender, object ea)
    {
        _baseEventsHandler.OnTickDone(sender, ea);
    }

    public void OnWeaponFired(object sender, object ea)
    {
        _baseEventsHandler.OnWeaponFired(sender, ea);
    }

    public void OnSmokeNadeStarted(object sender, object ea)
    {
        _baseEventsHandler.OnSmokeNadeStarted(sender, ea);
    }
}
