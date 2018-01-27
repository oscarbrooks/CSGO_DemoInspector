using SFB;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	void Start () {
        Application.runInBackground = true;
    }
	
	void Update () {
		
	}

    public void LoadDemoSelection()
    {
        SceneManager.LoadScene("demo_selection");
    }

    public void OpenFileDialog()
    {
        var extensions = new[] {
            new ExtensionFilter("Demo Files", "dem")
        };

        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false)[0];

        GameManager.Instance.CurrentFile = path;

        SceneManager.LoadScene("demo_viewer");
    }
}
