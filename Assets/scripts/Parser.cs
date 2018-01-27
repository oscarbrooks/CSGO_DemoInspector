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

    private bool _parsingFinished;
    private Image _progressImage;
    private EventsHandler _eventsHandler;
    private Queue<ParserEvent<object>> _parsingEventsQueue = new Queue<ParserEvent<object>>();

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
                parsingEvent.Action.Invoke(parsingEvent.Sender, parsingEvent.EventArgs);
            }
        }
        

        if (!_parsingFinished) UIManager.Instance.ParsingProgressLoaderUI?.UpdateProgress(Progress);
    }

    public void ParseFile () {

        if (!File.Exists(GameManager.Instance.CurrentFile)) return;

        _eventsHandler = new EventsHandler(this);

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
                parser.HeaderParsed += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnHeaderParsed, s, e));
                parser.RoundOfficiallyEnd += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnRoundEnd, s, e));
                parser.RoundStart += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnRoundStart, s, e));
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
        _parsingFinished = true;
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => UIManager.Instance.ParsingProgressLoaderUI.OnProgressComplete(), new { }, new { }));
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => PlayersManager.Instance.UpdatePlayers(PlaybackManager.Instance.Frames[0].Players), new { }, new { }));
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => UIManager.Instance.OnParsingComplete(), new { }, new { }));
        PlaybackManager.Instance.OnParsingComplete();
    }
}

public struct ParserEvent<T>
{
    public Action<object, object> Action { get; set; }
    public object Sender { get; set; }
    public object EventArgs { get; set; }

    public ParserEvent(Action<object, object> action, object sender, object eventArgs) {
        Action = action;
        Sender = sender;
        EventArgs = eventArgs;
    }
}
