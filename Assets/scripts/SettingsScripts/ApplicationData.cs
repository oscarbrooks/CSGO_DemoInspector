using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ApplicationData {
    [JsonProperty("demoFolderPath")]
    public List<string> DemoFolderPaths = new List<string>();

    public static ApplicationData DefaultApplicationData()
    {
        return new ApplicationData();
    }
}
