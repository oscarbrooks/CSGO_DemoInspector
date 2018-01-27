using DemoInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NadeGraphicsManager : SingletonMonoBehaviour<NadeGraphicsManager> {

    public const int PlaybackScale = GraphicsManager.PlaybackScale;

    public GameObject NadePrefab;
    public GameObject NadeProjectilePrefab;

    private Dictionary<Guid, NadeProjectileGraphics> _nadeProjectiles = new Dictionary<Guid, NadeProjectileGraphics>();

    private void Start () {
	}
	
	private void Update () {
		
	}

    public void UpdateNadeProjectileFrame(NadeProjectileFrame frame, Map map)
    {
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
        }

        foreach (Guid guid in leftOverGuids)
        {
            var nadeGraphics = _nadeProjectiles[guid];
            _nadeProjectiles.Remove(guid);
            Destroy(nadeGraphics.gameObject);
        }

    }

    private void AddNadeProjectile(NadeProjectile nade, Map map)
    {
        var pos = (DemoInfoHelper.SourceToUnityVector(nade.GetPos()) / PlaybackScale) - map.Offset;

        var offset = Vector3.zero;

        var nadeProjectileClone = Instantiate(NadeProjectilePrefab);
        nadeProjectileClone.transform.position = pos + offset;

        var nadeGraphics = nadeProjectileClone.GetComponent<NadeProjectileGraphics>();
        nadeGraphics.NadeType = nade.Type;
        nadeGraphics.Offset = offset;
        nadeGraphics.Tickrate = GraphicsManager.Instance.Tickrate;

        _nadeProjectiles.Add(nade.Guid, nadeGraphics);
    }
}
