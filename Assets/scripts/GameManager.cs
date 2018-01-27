using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    
    public string CurrentFile { get; set; }

	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	void Update () {
		
	}
}
