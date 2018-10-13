using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoInfo;

public class NadeEffectGraphics : MonoBehaviour {

    public EquipmentElement NadeType;

    public int StartTick;
    public int Duration;
    public float EndTick => StartTick + MatchInfoManager.Instance.Tickrate * Duration;

    private GameObject _graphics;

    private const string _nadesFolder = "prefabs/utility/";

    private void Start()
    {
        switch (NadeType)
        {
            case EquipmentElement.Smoke:
                _graphics = InstantiateGraphics("smoke_grenade_effect");
                break;
            default:
                break;
        }
    }

    private GameObject InstantiateGraphics(string name)
    {
        return Instantiate(Resources.Load<GameObject>(_nadesFolder + name), transform);
    }
}
