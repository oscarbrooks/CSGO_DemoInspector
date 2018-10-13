using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    
    public string CurrentFile { get; set; }

	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	void Update () {
		
	}

    public void LoadDemoSelection()
    {
        SceneManager.LoadScene("demo_selection");
    }

    public void LoadMainMenu()
    {
        ResetApp();

        SceneManager.LoadScene("main_menu");
    }

    public void LoadDemo(string filePath)
    {
        CurrentFile = filePath;

        SceneManager.LoadScene("demo_viewer");
    }

    private void ResetApp()
    {
        CurrentFile = string.Empty;

        //GraphicsManager.Instance.ResetSelf();
        //MatchInfoManager.Instance.ResetSelf();
        //NadeGraphicsManager.Instance.ResetSelf();
        //PlaybackManager.Instance.ResetSelf();
        //PlayerHurtGraphicsManager.Instance.ResetSelf();
        //PlayersManager.Instance.ResetSelf();
        //UIManager.Instance.ResetSelf();
    }
}
