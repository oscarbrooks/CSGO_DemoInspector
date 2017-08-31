using SFB;
using UnityEngine;

public class MenuController : MonoBehaviour {
    


	void Start () {
		
	}
	
	void Update () {
		
	}

    public void OpenFileDialog()
    {
        var extensions = new[] {
            new ExtensionFilter("Demo Files", "dem")
        };

        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false)[0];

        Parser.Instance.ParseFile(path);
    }
}
