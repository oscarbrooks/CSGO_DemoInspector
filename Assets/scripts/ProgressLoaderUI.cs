using UnityEngine;
using UnityEngine.UI;

public class ProgressLoaderUI : MonoBehaviour {

    private Image _progressImage;
    private GameObject _progressInner;
    private Text _text;

    private string _info;

	void Start () {
        _progressInner = transform.Find("Progress").gameObject;
        _progressImage = _progressInner.GetComponent<Image>();
        _text = transform.Find("ParsingText").GetComponentInChildren<Text>();

    }
	
	void Update () {
		
	}

    public void UpdateProgress(float progress)
    {
        if (_progressImage == null) return;

        _progressImage.fillAmount = progress;

        UpdateText();
    }

    public void UpdateInfo(string info)
    {
        _info = info;
    }

    public void UpdateText()
    {
        var progress = string.Format("{0:0}", _progressImage.fillAmount * 100);
        var text = $"{progress}%";
        //text += _info;
        _text.text = text;
    }

    public void OnProgressComplete()
    {
        _progressInner.SetActive(false);
        var animator = transform.GetComponent<Animator>();
        animator.SetTrigger("ParsingComplete");
    }
}
