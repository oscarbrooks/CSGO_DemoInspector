using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphicsManager : SingletonMonoBehaviour<GraphicsManager> {

    public GameObject NadePrefab;

    public Map Map;
    public float Tickrate { get; set; } = 64;
    public const int PlaybackScale = 35;

    private GameObject MapPlane { get; set; }
    private GameObject Map3DLevel { get; set; }
    public PlaybackMode PlaybackMode { get; set; } = PlaybackMode.TwoD;

    private Vector3 _originalScale3D;
    private Vector3 _originalScale2D;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void SetMapRadar(string name)
    {
        var mapOverviews = GameObject.Find("GameContent").GetComponent<MapOverviews>();

        if (mapOverviews.UseSimpleRadar) name += "_sr";

        Map = mapOverviews.Maps.First(m => m.Name == name);

        MapPlane = GameObject.Find("MapOverviewRadar");
        MapPlane.GetComponent<Renderer>().material.mainTexture = Map.Texture;
        MapPlane.transform.localScale = _originalScale2D = Map.Scale;

        Map3DLevel = Instantiate(Map.Layout3D);
        _originalScale3D = Map3DLevel.transform.localScale;

        SwitchToPlaybackMode(PlaybackMode.TwoD);
    }

    public void SwitchToPlaybackMode(PlaybackMode playbackMode)
    {
        PlaybackMode = playbackMode;

        if (PlaybackMode == PlaybackMode.ThreeD)
        {
            Map3DLevel.SetActive(true);

            Map3DLevel.transform.localScale = ScaleZeroHeight(_originalScale3D);

            var animation3D = AnimateScale(Map3DLevel, _originalScale3D, 0.2f);
            StartCoroutine(animation3D);

            var animation2D = AnimateScale(MapPlane, Vector3.zero, 0.2f, () => {
                MapPlane.transform.localScale = _originalScale2D;
                MapPlane.SetActive(false);
            });

            StartCoroutine(animation2D);
        }
        else
        {
            var animation3D = AnimateScale(Map3DLevel, ScaleZeroHeight(_originalScale3D), 0.2f, () =>
            {
                Map3DLevel.transform.localScale = _originalScale3D;
                Map3DLevel.SetActive(false);
            });

            MapPlane.transform.localScale = Vector3.zero;

            MapPlane.SetActive(true);

            var animation2D = AnimateScale(MapPlane, _originalScale2D, 0.2f);


            StartCoroutine(animation3D);
            StartCoroutine(animation2D);
        }
    }

    private IEnumerator AnimateScale(GameObject obj, Vector3 targetScale, float seconds, Action callback = null)
    {
        var elapsedTime = 0.0f;
        var startScale = obj.transform.localScale;

        while (elapsedTime < seconds)
        {
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, (elapsedTime / seconds));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;

        callback?.Invoke();
    }

    private Vector3 ScaleZeroHeight(Vector3 originalScale)
    {
        return new Vector3(originalScale.x, 0, originalScale.z);
    }

    public void DisplayWeaponFireFrame(WeaponFireFrame frame)
    {
        for (int i = 0; i < frame.WeaponFires.Count; i++)
        {
            var weaponFire = frame.WeaponFires[i];
            var position = (weaponFire.ShooterPosition / PlaybackScale) - Map.Offset;
            WeaponGraphicsManager.Instance.CreateWeaponFireGraphics(weaponFire, position, PlaybackMode);
        }
    }

    public void DisplayPlayerHurtFrame(PlayerHurtFrame frame)
    {
        PlayerHurtGraphicsManager.Instance.UpdatePlayerHurtFrame(frame, PlaybackScale, Map.Offset);
    }

    public void DisplayNadeFrame(NadeThrowFrame frame)
    {
        for (int i = 0; i < frame.NadeThrows.Count; i++)
        {
            var nadeThrow = frame.NadeThrows[i];
            var nadeClone = Instantiate(NadePrefab);
            nadeClone.GetComponent<NadeGraphics>().NadeThrow = nadeThrow;
            var pos = (nadeThrow.Position / PlaybackScale) - Map.Offset;
            if (PlaybackMode == PlaybackMode.TwoD) pos.y = 3;
            nadeClone.transform.position = pos;
            nadeClone.name = nadeThrow.NadeType.ToString() + " - " + nadeThrow.Thrower;

            Destroy(nadeClone, 5);
        }
    }

    public void UpdateNadeProjectileFrame(NadeProjectileFrame frame)
    {
        NadeGraphicsManager.Instance.UpdateNadeProjectileFrame(frame, Map);
    }

}
