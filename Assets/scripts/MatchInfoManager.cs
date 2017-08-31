using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchInfoManager : SingletonMonoBehaviour<MatchInfoManager> {

    public float Tickrate { get; private set; } = 64;
    public List<Round> Rounds { get; set; } = new List<Round>();

    private void Start () {
    }

    private void Update () {
		
	}

    public void SetTickrate(float tickrate)
    {
        Tickrate = tickrate;
        GraphicsManager.Instance.Tickrate = Tickrate;
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

        UIManager.Instance.RoundsBarUI.AddRound(round.Number);
    }

    private void ClearRounds()
    {
        Rounds.Clear();
        UIManager.Instance.RoundsBarUI.ClearRounds();
    }
}
