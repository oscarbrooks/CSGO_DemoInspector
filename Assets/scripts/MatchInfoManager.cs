using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchInfoManager : MonoBehaviour {

    public float Tickrate { get; private set; } = 64;
    public List<Round> Rounds { get; set; } = new List<Round>();

    private GraphicsManager _graphicsManager;
    private UIManager _uiManager;

    private void Start () {
        _graphicsManager = GetComponent<GraphicsManager>();
        _uiManager = GetComponent<UIManager>();
    }

    private void Update () {
		
	}

    public void SetTickrate(float tickrate)
    {
        Tickrate = tickrate;
        _graphicsManager.Tickrate = Tickrate;
    }

    public void AddRound(int number, int tick)
    {
        var round = new Round()
        {
            Number = number,
            StartTick = tick
        };

        if(number == 1 && Rounds.Any()) ClearRounds();

        Rounds.Add(round);

        _uiManager.RoundsBarUI.AddRound(round.Number);
    }

    private void ClearRounds()
    {
        Rounds.Clear();
        _uiManager.RoundsBarUI.ClearRounds();
    }
}
