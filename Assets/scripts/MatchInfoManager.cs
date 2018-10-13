using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchInfoManager : SingletonMonoBehaviour<MatchInfoManager> {

    public float Tickrate { get; private set; } = 64;
    public List<Round> Rounds = new List<Round>();

    private void Start () {
    }

    private void Update () {
		
	}

    public void SetTickrate(float tickrate)
    {
        Tickrate = Mathf.Round(tickrate);
        GraphicsManager.Instance.Tickrate = Tickrate;
    }

    public void AddRound(int number, int tick)
    {
        var newRound = new Round()
        {
            Number = number,
            StartTick = tick
        };

        if(Rounds.Any() && newRound.Number == Rounds[0].Number) ClearRounds();

        if (!Rounds.Any(r => r.Number == number)) {
            Rounds.Add(newRound);
            UIManager.Instance.RoundsBarUI.AddRound(newRound.Number);
        }


    }

    private void ClearRounds()
    {
        Rounds.Clear();
        UIManager.Instance.RoundsBarUI.ClearRounds();
    }
}
