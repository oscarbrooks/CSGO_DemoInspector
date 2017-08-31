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
    public List<NadeThrowFrame> NadeThrowFrames { get; set; } = new List<NadeThrowFrame>();
    public List<NadeProjectileFrame> NadeProjectileFrames { get; set; } = new List<NadeProjectileFrame>();

    public bool IsPlaying { get; private set; } = false;
    public bool IsInitialized { get; private set; } = false;
    private bool _isReady = false;

    private float _tickrate = 32;
    private int currentFrame = 0;
    private int currentWeaponFrame = 0;
    private int currentNadeFrame = 0;
    private int currentNadeProjectileFrame = 0;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void Initialize(float tickrate, PartialPlayer[] players)
    {
        _tickrate = tickrate;
        GraphicsManager.Instance.CreatePlayers(players);
        IsInitialized = true;
    }

    public void OnParsingComplete()
    {
        _isReady = true;
    }

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
                //Round = Rounds.Last().Number,
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
        currentFrame = Mathf.Clamp(currentFrame + frames, 0, Frames.Count - 1);
        if (!IsPlaying) GraphicsManager.Instance.UpdatePlayers(Frames[currentFrame].Players);
    }

    public void SkipToRound(int roundNumber)
    {
        //var round = Rounds.Find(r => r.Number == roundNumber);
        var round = MatchInfoManager.Instance.Rounds.Find(r => r.Number == roundNumber);
        GoToTick(round.StartTick);
        if (!IsPlaying) GraphicsManager.Instance.UpdatePlayers(Frames[currentFrame].Players);
    }

    private void GoToTick(int tick)
    {
        var currentTick = Frames[currentFrame].Tick;
        var difference = tick - currentTick;
        currentFrame = Mathf.Clamp(currentFrame + difference, 0, Frames.Count - 1);
    }

    public IEnumerator IterateThroughFrames()
    {
        while (true)
        {
            if (currentFrame >= Frames.Count - 2) yield return new WaitForSeconds(1 / _tickrate);
            var frame = Frames[currentFrame];
            GraphicsManager.Instance.UpdatePlayers(frame.Players);

            AlignFrames(frame.Tick);
            UpdateFrames(frame);

            currentFrame++;
            yield return new WaitForSeconds((1 / _tickrate) * TimeScale);
        }
    }

    private void UpdateFrames(Frame frame)
    {
        if (frame.Tick == WeaponFiredFrames[currentWeaponFrame].Tick)
        {
            GraphicsManager.Instance.DisplayWeaponFireFrame(WeaponFiredFrames[currentWeaponFrame]);
            currentWeaponFrame++;
        }

        if (frame.Tick == NadeThrowFrames[currentNadeFrame].Tick)
        {
            GraphicsManager.Instance.DisplayNadeFrame(NadeThrowFrames[currentNadeFrame]);
            currentNadeFrame++;
        }

        if (frame.Tick == NadeProjectileFrames[currentNadeProjectileFrame].Tick)
        {
            GraphicsManager.Instance.UpdateNadeProjectileFrame(NadeProjectileFrames[currentNadeProjectileFrame]);
            currentNadeProjectileFrame++;
        }
    }

    private void AlignFrames(int currentTick)
    {
        AlignFrame(currentTick, WeaponFiredFrames, ref currentWeaponFrame);
        AlignFrame(currentTick, NadeThrowFrames, ref currentNadeFrame);
        AlignFrame(currentTick, NadeProjectileFrames, ref currentNadeProjectileFrame);
    }

    private void AlignFrame<T>(int currentTick, List<T> frames, ref int currentFrameIndex) where T : IFrame
    {
        var frame = frames[currentFrameIndex];
        while (frame.Tick < currentTick)
        {
            frame = frames[currentFrameIndex++];
        }
        while (frame.Tick > currentTick && currentFrameIndex > 0)
        {
            frame = frames[currentFrameIndex--];
        }
    }

}
