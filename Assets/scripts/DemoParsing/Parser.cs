using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using DemoInfo;
using System;
using System.Collections.Generic;

public class Parser : SingletonMonoBehaviour<Parser> {
    
    public float Progress;
    public bool MatchStarted = false;

    private Image _progressImage;
    private IDemoEventHandler _eventsHandler;
    private Queue<ParserEvent<object>> _parsingEventsQueue = new Queue<ParserEvent<object>>();

    [SerializeField]
    public UI.LoadingController LoadingControllerUI;

    private void Start()
    {
        ParseFile();
    }

    private void Update () {
        if (_parsingEventsQueue.Count > 0)
        {
            for (int i = 0; i < _parsingEventsQueue.Count; i++)
            {
                var parsingEvent = _parsingEventsQueue.Dequeue();
                if(parsingEvent.Action == null)
                {
                    Debug.Log($"{i}: action '{parsingEvent.Name}' is null, {parsingEvent.EventArgs} | {parsingEvent.Sender}");
                }
                parsingEvent.Action.Invoke(parsingEvent.Sender, parsingEvent.EventArgs);
            }
        }
    }

    public void ParseFile () {
        _parsingEventsQueue.Clear();

        if (!File.Exists(GameManager.Instance.CurrentFile)) return;

        _eventsHandler = EventsHandlerProvider.GetEventsHandler(GameManager.Instance.CurrentFile, this);

        UIManager.Instance.StartParsing();

        var parsingThread = new Thread(new ThreadStart(() => StartParseThread(GameManager.Instance.CurrentFile)));

        try
        {
            parsingThread.Start();
        }
        catch (ThreadStateException)
        {
            Debug.Log("Parsing thread aborted");
        }

    }

    private void StartParseThread(string filePath)
    {
        using (var filestream = File.OpenRead(filePath))
        {
            using (var parser = new DemoParser(filestream))
            {
                parser.HeaderParsed += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnHeaderParsed, s, e, "header PArsed"));
                parser.RoundOfficiallyEnd += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnRoundEnd, s, e, "rounf official end"));
                parser.RoundStart += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnRoundStart, s, e, "round start"));
                parser.PlayerTeam += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnPlayerTeam, s, e, "Player team"));
                parser.SmokeNadeStarted += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnSmokeNadeStarted, s, e));
                parser.MatchStarted += (s, e) => MatchStarted = true;
                parser.WeaponFired += _eventsHandler.OnWeaponFired;
                parser.PlayerHurt += _eventsHandler.OnPlayerHurt;
                parser.TickDone += _eventsHandler.OnTickDone;

                parser.ParseHeader();
                parser.ParseToEnd();

                OnParsingComplete();

                Thread.CurrentThread.Abort();
            }
        }
    }

    private void OnParsingComplete()
    {
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => UIManager.Instance.ParsingProgressLoaderUI.OnProgressComplete(), new { }, new { }, "ProgressLoaderUI complete"));
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => PlayersManager.Instance.UpdatePlayers(PlaybackManager.Instance.Frames[0].Players), new { }, new { }));
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => PlaybackManager.Instance.SkipToRound(1), new { }, new { }));
        //_parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => UIManager.Instance.OnParsingComplete(), new { }, new { }, "UI on complete"));
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => LoadingControllerUI.OnParsingComplete(), new { }, new { }, "UI on complete"));
        PlaybackManager.Instance.OnParsingComplete();
    }

    public void Enqueue(Action<object, object> action, string name)
    {
        _parsingEventsQueue.Enqueue(new ParserEvent<object>(action, new { }, new { }, name));
    }
}

public struct ParserEvent<T>
{
    public Action<object, object> Action { get; set; }
    public object Sender { get; set; }
    public object EventArgs { get; set; }

    public string Name { get; set; }

    public ParserEvent(Action<object, object> action, object sender, object eventArgs, string name = "parser event") {
        Action = action;
        Sender = sender;
        EventArgs = eventArgs;
        Name = name;
    }
}
