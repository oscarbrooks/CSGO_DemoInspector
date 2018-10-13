using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SettingsManager : SingletonMonoBehaviour<SettingsManager> {

    public ApplicationData ApplicationData { get; set; }

    private string _filePath;

	private void Awake () {
        Initialize();

        _filePath = Application.persistentDataPath + "/applicationData.json";
        ApplicationData = LoadData();
	}
	
	private void Update () {
		
	}

    public ApplicationData Save() => WriteToFile(_filePath, ApplicationData);

    private ApplicationData LoadData()
    {
        ApplicationData data;

        if (!File.Exists(_filePath)) data = WriteToFile(_filePath, ApplicationData.DefaultApplicationData());

        using (StreamReader file = File.OpenText(_filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            data = (ApplicationData)serializer.Deserialize(file, typeof(ApplicationData));
        }

        return data;
    }

    private ApplicationData WriteToFile(string filePath, ApplicationData data)
    {
        using (StreamWriter file = File.CreateText(_filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, data);
        }

        return data;
    }
}
