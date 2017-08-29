using System.IO;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DemoInfo;
using System;
using System.Collections.Generic;

public class Parser : MonoBehaviour {

    public string filePath;
    
    private PlaybackManager _playbackManager;
    private GraphicsManager _graphicsManager;
    private UIManager _uiManager;

    private EventsHandler _eventsHandler;
    private Queue<ParserEvent<object>> _parsingEventsQueue = new Queue<ParserEvent<object>>();

    private Image _progressImage;
    public float Progress;
    public bool MatchStarted = false;
    private bool _parsingFinished;

    void Start () {
        _playbackManager = GetComponent<PlaybackManager>();
        _graphicsManager = GetComponent<GraphicsManager>();
        _uiManager = GetComponent<UIManager>();

        _eventsHandler = new EventsHandler(this);

        if (!File.Exists(filePath)) return;

        var parsingThread = new Thread(new ThreadStart(ParseDemo));

        try
        {
            parsingThread.Start();
        }
        catch (ThreadStateException)
        {
            Debug.Log("Parsing thread aborted");
        }

    }

    void Update () {
        if (_parsingEventsQueue.Count > 0)
        {
            for (int i = 0; i < _parsingEventsQueue.Count; i++)
            {
                var parsingEvent = _parsingEventsQueue.Dequeue();
                parsingEvent.Action.Invoke(parsingEvent.Sender, parsingEvent.EventArgs);
            }
        }

        if (!_parsingFinished) _uiManager.ParsingProgressLoaderUI.UpdateProgress(Progress);
    }

    private void ParseDemo()
    {
        using (var filestream = File.OpenRead(filePath))
        {
            using (var parser = new DemoParser(filestream))
            {
                parser.HeaderParsed += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnHeaderParsed, s, e));
                parser.RoundOfficiallyEnd += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnRoundEnd, s, e));
                parser.RoundStart += (s, e) => _parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnRoundStart, s, e));
                parser.MatchStarted += (s, e) => MatchStarted = true;
                parser.WinPanelMatch += (s, e) =>
                {
                    //var players = parser.PlayingParticipants.Select(p => new PartialPlayer(p)).ToArray();
                    //_parsingEventsQueue.Enqueue(new ParserEvent<object>(_eventsHandler.OnGameEnd, s, players));
                };
                parser.WeaponFired += _eventsHandler.OnWeaponFired;
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
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => _uiManager.ParsingProgressLoaderUI.OnProgressComplete(), new { }, new { }));
        _parsingEventsQueue.Enqueue(new ParserEvent<object>((s, e) => _graphicsManager.UpdatePlayers(_playbackManager.Frames[0].Players), new { }, new { }));
        _playbackManager.OnParsingComplete();
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
