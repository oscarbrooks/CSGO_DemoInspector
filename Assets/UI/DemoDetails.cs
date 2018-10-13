using UnityEngine;
using UnityEngine.UI;
using DemoInfo;
using CSharpFunctionalExtensions;
using System.IO;

public class DemoDetails : MonoBehaviour {
    
    public DemoHeader Header { get; set; }
    public string FilePath { get; set; }

    [SerializeField]
    private Button _loadDemoButton;

	private void Start () {
        _loadDemoButton.onClick.AddListener(() => GameManager.Instance.LoadDemo(FilePath));
	}
	
	private void Update () {
		
	}

    public void Initialize(string filePath)
    {
        var matchInfo = "Unable to read match information";

        var headerResult = DemoHeaderParser.ParseHeader(filePath)
            .OnSuccess(header => matchInfo = $"{header.MapName} - {DemoHeaderParser.GetDemoSource(header.ServerName).Name}");

        if (headerResult.IsFailure) return;

        FilePath = filePath;

        transform.Find("TextContainer/Text").GetComponent<Text>().text = $"{matchInfo} | {Path.GetFileName(filePath)}";
    }
}
