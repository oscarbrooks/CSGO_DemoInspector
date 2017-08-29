using UnityEngine;
using UnityEngine.UI;

public class RoundsBarUI : MonoBehaviour {
    public GameObject RoundButtonPrefab;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void AddRound(int number)
    {
        var roundButtonClone = Instantiate(RoundButtonPrefab);
        roundButtonClone.transform.SetParent(transform, false);
        roundButtonClone.GetComponent<RoundsButtonUI>().RoundNumber = number;
        roundButtonClone.GetComponentInChildren<Text>().text = number.ToString();
    }

    public void ClearRounds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
