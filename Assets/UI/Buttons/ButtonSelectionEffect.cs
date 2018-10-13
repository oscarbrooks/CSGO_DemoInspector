using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
[AddComponentMenu("UI/Effects/Button Selection")]
public class ButtonSelectionEffect : MonoBehaviour {

    private RectTransform _rectTransform;
    private Vector2 _startSize;
    private Vector2 _startPosition;


    [SerializeField]
    public bool IsSelected = false;

    [Header("Underline")]
    private Shadow _shadow;
    [SerializeField]
    private int _underlineThickness = 1;
    [SerializeField]
    private Color _color = Color.black;

	private void Start () {
        _rectTransform = GetComponent<RectTransform>();
        _startSize = _rectTransform.sizeDelta;
        _startPosition = _rectTransform.anchoredPosition;

        _shadow = gameObject.AddComponent<Shadow>();
        _shadow.effectColor = _color;

        SetSelected(IsSelected);
	}
	
	private void Update () {
		
	}

    public void AddListener(UnityEngine.Events.UnityAction call)
    {
        GetComponent<Button>().onClick.AddListener(call);
    }

    public bool SetSelected(bool shouldSelect)
        => shouldSelect ? EnableUnderline() : DisableUnderline();

    private bool EnableUnderline()
    {
        _shadow.effectDistance = new Vector2(0, -_underlineThickness);

        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _startSize.y - _underlineThickness);

        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _startPosition.y + _underlineThickness / 2);

        return IsSelected = true;
    }

    private bool DisableUnderline()
    {
        _shadow.effectDistance = Vector2.zero;

        _rectTransform.sizeDelta = _startSize;

        _rectTransform.anchoredPosition = _startPosition;

        return IsSelected = false;
    }

}
