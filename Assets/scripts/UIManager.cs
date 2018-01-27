using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager> {

    public GameObject Canvas;
    public GameObject progressLoaderPrefab;

    public ProgressLoaderUI ParsingProgressLoaderUI { get; private set; }
    public RoundsBarUI RoundsBarUI { get; private set; }

    private void Start () {
        Canvas = GameObject.Find("Canvas");
        RoundsBarUI = GameObject.Find("RoundsBar").GetComponent<RoundsBarUI>();
    }
	
	private void Update () {
		
	}

    public void StartParsing()
    {
        Canvas = GameObject.Find("Canvas");

        var progressLoaderClone = Instantiate(progressLoaderPrefab);

        progressLoaderClone.transform.SetParent(Canvas.transform, false);

        ParsingProgressLoaderUI = progressLoaderClone.GetComponent<ProgressLoaderUI>();
    }

    public void OnParsingComplete()
    {
        var background = GameObject.Find("UIBackground").GetComponent<Image>();
        StartCoroutine(FadeBackground(background));
    }

    private IEnumerator FadeBackground(Image background)
    {
        for (float f = 1f; f >= 0; f -= 0.02f)
        {
            var color = background.color;
            color.a = f;
            background.color = color;
            yield return null;
        }
        background.gameObject.SetActive(false);
    }
}
