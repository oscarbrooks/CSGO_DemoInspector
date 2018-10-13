using DemoInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NadeGraphicsManager : SingletonMonoBehaviour<NadeGraphicsManager> {

    public const int PlaybackScale = GraphicsManager.PlaybackScale;

    public GameObject NadePrefab;
    public GameObject NadeEffectPrefab;
    public GameObject NadeProjectilePrefab;

    public int CurrentTick;

    private Dictionary<Guid, NadeProjectileGraphics> _nadeProjectiles = new Dictionary<Guid, NadeProjectileGraphics>();
    private Dictionary<Guid, NadeEffectGraphics> _nadeEffects = new Dictionary<Guid, NadeEffectGraphics>();
    public Dictionary<Guid, TrailRendererFollower> NadeTrails = new Dictionary<Guid, TrailRendererFollower>();

    [SerializeField]
    private Text _debugText;

    private void Start () {
	}
	
	private void Update () {
		
	}

    public void UpdateNadeProjectileFrame(NadeProjectileFrame frame, Map map)
    {
        _debugText.text = $"There should be {frame.NadeProjectiles.Count} nades";

        var leftOverGuids = _nadeProjectiles.Keys.ToList();

        for (int i = 0; i < frame.NadeProjectiles.Count; i++)
        {
            var nade = frame.NadeProjectiles[i];
            if (leftOverGuids.Contains(nade.Guid))
            {
                leftOverGuids.Remove(nade.Guid);
            }
            else
            {
                AddNadeProjectile(nade, map);
            }
            _nadeProjectiles[nade.Guid].UpdatePosition((DemoInfoHelper.SourceToUnityVector(nade.GetPos()) / PlaybackScale) - map.Offset);
            EnsureNadeHasTrail(nade.Guid);
        }

        foreach (Guid guid in leftOverGuids) RemoveNadeProjectile(guid);

    }

    public void UpdateNadeEffects(Frame currentFrame, NadeEffectFrame currentNadeFrame)
    {
        CurrentTick = currentFrame.Tick;

        var currentEffectGuids = _nadeEffects.Keys.ToList();

        if(currentFrame.Tick == currentNadeFrame.Tick)
        {
            for (int i = 0; i < currentNadeFrame.NadeEffects.Count; i++)
            {
                var nade = currentNadeFrame.NadeEffects[i];
                if (currentEffectGuids.Contains(nade.Guid))
                {
                    currentEffectGuids.Remove(nade.Guid);
                }
                else
                {
                    AddNadeEffect(nade, currentNadeFrame.Tick);
                }
            }
        }

        foreach (Guid guid in currentEffectGuids)
        {
            var nadeGraphics = _nadeEffects[guid];
            if (currentFrame.Tick < nadeGraphics.StartTick || currentFrame.Tick > nadeGraphics.EndTick) RemoveNadeEffect(guid);
        }
    }

    public void ClearNades()
    {
        if (_nadeProjectiles.Count == 0 && NadeTrails.Count == 0) return;

        ClearNadeTrails();

        foreach (var guid in _nadeProjectiles.Keys.ToArray()) RemoveNadeProjectile(guid);

        _nadeProjectiles.Clear();
    }

    public void ClearNadeTrails()
    {
        foreach (var trail in NadeTrails.Values) Destroy(trail.gameObject);

        NadeTrails.Clear();
    }

    private void AddNadeProjectile(NadeProjectile nade, Map map)
    {
        var pos = (DemoInfoHelper.SourceToUnityVector(nade.GetPos()) / PlaybackScale) - map.Offset;

        var offset = Vector3.zero;

        var nadeProjectileClone = Instantiate(NadeProjectilePrefab);

        nadeProjectileClone.name = $"NadeProjectile {nade.Guid}";

        nadeProjectileClone.transform.position = pos + offset;

        var nadeGraphics = nadeProjectileClone.GetComponent<NadeProjectileGraphics>();
        nadeGraphics.NadeType = nade.Type;
        nadeGraphics.Offset = offset;
        nadeGraphics.Tickrate = GraphicsManager.Instance.Tickrate;

        _nadeProjectiles.Add(nade.Guid, nadeGraphics);
        NadeTrails.Add(nade.Guid, nadeGraphics.CreateTrail());
    }

    private void AddNadeEffect(NadeEffect nade, int tick)
    {
        var offset = Vector3.zero;

        var nadeEffectClone = Instantiate(NadeEffectPrefab);

        nadeEffectClone.name = $"NadeEffect {nade.Guid}";

        nadeEffectClone.transform.position = nade.Position;

        var nadeGraphics = nadeEffectClone.GetComponent<NadeEffectGraphics>();

        nadeGraphics.NadeType = nade.NadeType;

        nadeGraphics.Duration = nade.Duration;

        nadeGraphics.StartTick = tick;

        _nadeEffects.Add(nade.Guid, nadeGraphics);
    }

    private void EnsureNadeHasTrail(Guid guid)
    {
        var nadeGraphics = _nadeProjectiles[guid];

        TrailRendererFollower nadeTrail;
        if(NadeTrails.TryGetValue(guid, out nadeTrail))
        {
            if (nadeGraphics == null) nadeGraphics.Trail = nadeTrail;
            return;
        }

        if (nadeGraphics.Trail == null) NadeTrails.Add(guid, nadeGraphics.CreateTrail());
        else NadeTrails.Add(guid, nadeGraphics.Trail);
    }

    private void RemoveNadeProjectile(Guid guid)
    {
        var nadeGraphics = _nadeProjectiles[guid];
        _nadeProjectiles.Remove(guid);
        Destroy(nadeGraphics.gameObject);
        TryDestroyNadeTrail(guid);
    }

    private void RemoveNadeEffect(Guid guid)
    {
        var nadeGraphics = _nadeEffects[guid];
        _nadeEffects.Remove(guid);

        Destroy(nadeGraphics.gameObject);
    }

    private void TryDestroyNadeTrail(Guid guid)
    {
        TrailRendererFollower nadeTrail;
        if (NadeTrails.TryGetValue(guid, out nadeTrail))
        {
            if(nadeTrail != null) Destroy(nadeTrail.gameObject, nadeTrail.TrailRenderer.time);
            NadeTrails.Remove(guid);
        }
    }
}
