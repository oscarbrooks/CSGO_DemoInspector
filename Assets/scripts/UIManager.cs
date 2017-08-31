﻿using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager> {

    public ProgressLoaderUI ParsingProgressLoaderUI { get; private set; }
    public RoundsBarUI RoundsBarUI { get; private set; }

    void Start () {
        ParsingProgressLoaderUI = GameObject.Find("ParsingProgressLoader").GetComponent<ProgressLoaderUI>();
        RoundsBarUI = GameObject.Find("RoundsBar").GetComponent<RoundsBarUI>();
    }
	
	void Update () {
		
	}
}
