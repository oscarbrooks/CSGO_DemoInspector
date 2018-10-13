using SFB;
using System.IO;
using System.Linq;
using UnityEngine;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private DemoFilesUIController _demoFilesControllerUI;

	private void Start () {
        Application.runInBackground = true;

        InitialDisplayDemos();
    }

    private void Update () {
		
	}

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenFileDialog()
    {
        var extensions = new[] {
            new ExtensionFilter("Demo Files", "dem")
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        if (!paths.Any()) return;

        GameManager.Instance.LoadDemo(paths[0]);
    }

    public void OpenFolderDialog()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Add CS:GO Demos Folder", "", false);

        if (!paths.Any()) return;

        AddDemoFolderPath(paths[0]);
    }

    private void InitialDisplayDemos()
    {
        var demoFolderPaths = SettingsManager.Instance.ApplicationData.DemoFolderPaths;

        _demoFilesControllerUI.AddDemoFolders(demoFolderPaths);
    }

    public void AddDemoFolderPath(string folderPath)
    {
        if (!Directory.Exists(folderPath)) return;

        if (SettingsManager.Instance.ApplicationData.DemoFolderPaths.Contains(folderPath)) return;

        Debug.Log("Adding folder " + folderPath);

        SettingsManager.Instance.ApplicationData.DemoFolderPaths.Add(folderPath);

        SettingsManager.Instance.Save();
    }
}
