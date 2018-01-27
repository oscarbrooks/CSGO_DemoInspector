using UnityEngine;

public class MapOverviews : MonoBehaviour {

    public bool UseSimpleRadar;
    public Map[] Maps;

	void Start () {
		
	}
	
	void Update () {
		
	}
}

[System.Serializable]
public struct Map
{
    public string Name;
    public Texture Texture;
    public GameObject Layout3D;
    public Vector3 Offset;
    public Vector3 Scale;
}
