using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    public static T Instance = null;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (Instance == null)
            Instance = this as T;
        else if (Instance != this as T)
            Destroy(this);
    }
}
