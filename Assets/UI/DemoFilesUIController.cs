using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DemoFilesUIController : MonoBehaviour {

    [SerializeField]
    private GameObject _demoFilesGroupPrefab;
    [SerializeField]
    private GameObject _demoFilePanelPrefab;

    [SerializeField]
    private Transform _demoFilesSection;

    private List<GameObject> _demoFiles;

    private void Start () {
    }
	
	private void Update () {
		
	}

    public void AddDemoFolders(IEnumerable<string> folderPaths)
    {
        foreach (var path in folderPaths) AddDemoFolder(path);
    }

    public void AddDemoFolder(string folderPath)
    {
        var demoFilesGroup = Instantiate(_demoFilesGroupPrefab, _demoFilesSection);

        demoFilesGroup.transform.Find("Title").GetComponentInChildren<Text>().text = folderPath;

        var demoFiles = Directory.EnumerateFiles(folderPath)
            .OrderBy(file => new FileInfo(file).Length);

        if (!demoFiles.Any()) return;

        var demoFilesContainer = demoFilesGroup.transform.Find("DemoFiles");

        demoFilesContainer.transform.Find("PlaceholderDemoFilePanel").gameObject.SetActive(false);

        foreach (var file in demoFiles)
        {
            if (Path.GetExtension(file) != ".dem") continue;

            var demoDetails = Instantiate(_demoFilePanelPrefab, demoFilesContainer.transform).GetComponent<DemoDetails>();

            demoDetails.Initialize(file);
        }
    }
}
