using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DemoInfo;

public class PlaybackManager : SingletonMonoBehaviour<PlaybackManager> {
    public float TimeScale = 1;

    public List<Round> Rounds { get; set; } = new List<Round>();
    public List<Frame> Frames { get; set; } = new List<Frame>();
    public List<WeaponFireFrame> WeaponFiredFrames { get; set; } = new List<WeaponFireFrame>();
    public List<PlayerHurtFrame> PlayerHurtFrames { get; set; } = new List<PlayerHurtFrame>();

    public bool IsPlaying { get; private set; } = false;
    public bool IsInitialized { get; private set; } = false;
    private bool _isReady = false;

    private float _tickrate = 128;
    public int CurrentFrame = 0;
    private int currentWeaponFrame = 0;
    private int currentNadeFrame = 0;
    private int currentNadeEffectFrame = 0;
    private int currentNadeProjectileFrame = 0;
    private int currentPlayerHurtFrame = 0;
    
    private int _framesToIterate = 0;

    private CustomFixedUpdate _customFixedUpdate;

    private void Start () {
		
	}
	
	private void Update () {
        if (!IsInitialized || !IsPlaying) return;

        _customFixedUpdate.Update(Time.deltaTime);
	}

    public void Initialize(PartialPlayer[] players)
    {
        _tickrate = MatchInfoManager.Instance.Tickrate;

        Debug.Log($"initializing playback manager {_tickrate}");

        SetTimescale(1);

        IsInitialized = true;
    }

    public void OnParsingComplete()
    {
        _isReady = true;
    }

    public void SetTimescale(float timescale)
    {
        TimeScale = timescale;
        _customFixedUpdate = new CustomFixedUpdate((1 / _tickrate) / TimeScale, () => _framesToIterate++);
    }

    public void TogglePlayPause()
    {
        if (!_isReady) return;
        if(IsPlaying)
        {
            StopCoroutine("IterateThroughFrames");
            IsPlaying = false;
        }
        else
        {
            StartCoroutine("IterateThroughFrames");
            IsPlaying = true;
        }
    }

    public void Skip(int seconds)
    {
        if (!_isReady) return;
        var frames = Mathf.RoundToInt(seconds * _tickrate);
        CurrentFrame = Mathf.Clamp(CurrentFrame + frames, 0, Frames.Count - 1);
        if (!IsPlaying) PlayersManager.Instance.UpdatePlayers(Frames[CurrentFrame].Players);
    }

    public void SkipToRound(int roundNumber)
    {
        var round = MatchInfoManager.Instance.Rounds.Find(r => r.Number == roundNumber);
        GoToTick(round.StartTick + 1);

        if (!IsPlaying) PlayersManager.Instance.UpdatePlayers(Frames[CurrentFrame].Players);
    }

    private void GoToTick(int tick)
    {
        var currentTick = Frames[CurrentFrame].Tick;

        var difference = tick - currentTick;

        CurrentFrame = Mathf.Clamp(CurrentFrame + difference, 0, Frames.Count - 1);

        NadeGraphicsManager.Instance.ClearNadeTrails();
    }

    public IEnumerator IterateThroughFrames()
    {
        while (true)
        {
            for (int i = 0; i < _framesToIterate; i++)
            {
                IterateFrame();
            }

            _framesToIterate = 0;

            //if (CurrentFrame == 8000) Debug.Log($"1. tickrate: {_tickrate} | timescale {TimeScale} | wait for {(1 / _tickrate) * TimeScale}s | TIME {Time.time}");
            //if (CurrentFrame == 8001) Debug.Log($"2. tickrate: {_tickrate} | timescale {TimeScale} | TIME {Time.time}");

            //yield return new WaitForSeconds((1 / _tickrate) * TimeScale);
            yield return null;
        }
    }

    private void IterateFrame()
    {
        if (CurrentFrame >= Frames.Count - 2) return;

        var frame = Frames[CurrentFrame];

        PlayersManager.Instance.UpdatePlayers(frame.Players);

        AlignFrames(frame.Tick);

        UpdateFrames(frame);

        CurrentFrame++;
    }

    private void UpdateFrames(Frame frame)
    {
        if (frame.Tick == WeaponFiredFrames[currentWeaponFrame].Tick)
        {
            GraphicsManager.Instance.DisplayWeaponFireFrame(WeaponFiredFrames[currentWeaponFrame]);
            currentWeaponFrame++;
        }

        if (frame.Tick == NadePlaybackManager.Instance.NadeThrowFrames[currentNadeFrame].Tick)
        {
            GraphicsManager.Instance.DisplayNadeFrame(NadePlaybackManager.Instance.NadeThrowFrames[currentNadeFrame]);
            currentNadeFrame++;
        }
        
        NadeGraphicsManager.Instance.UpdateNadeEffects(frame, NadePlaybackManager.Instance.NadeEffectFrames[currentNadeEffectFrame]);
        if (frame.Tick == NadePlaybackManager.Instance.NadeEffectFrames[currentNadeEffectFrame].Tick) currentNadeEffectFrame++;

        if (frame.Tick == NadePlaybackManager.Instance.NadeProjectileFrames[currentNadeProjectileFrame].Tick)
        {
            GraphicsManager.Instance.UpdateNadeProjectileFrame(NadePlaybackManager.Instance.NadeProjectileFrames[currentNadeProjectileFrame]);
            currentNadeProjectileFrame++;
        }
        else
        {
            NadeGraphicsManager.Instance.ClearNades();
        }


        if (frame.Tick == PlayerHurtFrames[currentPlayerHurtFrame].Tick)
        {
            GraphicsManager.Instance.DisplayPlayerHurtFrame(PlayerHurtFrames[currentPlayerHurtFrame]);
            currentPlayerHurtFrame++;
        }
    }

    private void AlignFrames(int currentTick)
    {
        AlignFrame(currentTick, WeaponFiredFrames, ref currentWeaponFrame);
        AlignFrame(currentTick, NadePlaybackManager.Instance.NadeThrowFrames, ref currentNadeFrame);
        AlignFrame(currentTick, NadePlaybackManager.Instance.NadeProjectileFrames, ref currentNadeProjectileFrame);
        AlignFrame(currentTick, NadePlaybackManager.Instance.NadeEffectFrames, ref currentNadeEffectFrame);
        AlignFrame(currentTick, PlayerHurtFrames, ref currentPlayerHurtFrame);
    }

    private void AlignFrame<T>(int currentTick, List<T> frames, ref int currentFrameIndex) where T : IFrame
    {
        if(currentFrameIndex >= frames.Count)
        {
            currentFrameIndex = frames.Count - 1;
            return;
        }

        var frame = frames[currentFrameIndex];

        while (frame.Tick > currentTick && currentFrameIndex > 0)
        {
            frame = frames[--currentFrameIndex];
        }

        while (currentFrameIndex != frames.Count - 1 && frame.Tick < currentTick)
        {
            frame = frames[++currentFrameIndex];
        }
    }

}
