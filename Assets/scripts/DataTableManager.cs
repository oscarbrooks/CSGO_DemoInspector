using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DemoInfo;

public class DataTableManager : MonoBehaviour {

    public GameObject Table;
    public GameObject RowPrefab;

    private Player[] _players;

	private void Start () {
	}
	
	private void Update () {
		
	}

    public void SetPlayers(Player[] players)
    {
        _players = players;
        DrawPlayerTable();
    }

    public void SortByKills()
    {
        _players = _players.OrderBy(p => p.Team)
            .ThenByDescending(p => p.AdditionaInformations.Kills)
            .ToArray();
        DrawPlayerTable();
    }

    private void DrawPlayerTable()
    {
        DeletePlayerRows();
        for (int i = 0; i < _players.Length; i++)
        {
            var player = _players[i];
            AddPlayerRow(player);
        }
    }

    private void DeletePlayerRows()
    {
        if (Table.transform.childCount <= 0) return;

        for (int i = 0; i < Table.transform.childCount; i++)
        {
            var row = Table.transform.GetChild(i).gameObject;
            if(row.name != "HeaderRow") Destroy(row);
        }
    }

    public void AddPlayerRow(Player player)
    {
        GameObject rowClone = Instantiate(RowPrefab);
        rowClone.transform.SetParent(Table.transform, false);
        rowClone.transform.Find("NameCell").GetComponentInChildren<Text>().text = player.Name;
        rowClone.transform.Find("KillsCell").GetComponentInChildren<Text>().text = player.AdditionaInformations.Kills.ToString();
        rowClone.transform.Find("AssistsCell").GetComponentInChildren<Text>().text = player.AdditionaInformations.Assists.ToString();
        rowClone.transform.Find("DeathsCell").GetComponentInChildren<Text>().text = player.AdditionaInformations.Deaths.ToString();
    }
}
