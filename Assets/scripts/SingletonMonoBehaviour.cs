using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : class {

    public static T Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else if (Instance != this as T)
            Destroy(gameObject);
    }
}
