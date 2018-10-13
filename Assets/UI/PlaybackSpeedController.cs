using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PlaybackSpeedController : MonoBehaviour {

    private Slider _slider;

    [SerializeField]
    private Text _text;

    private readonly float[] valueMap = { 0.1f, 0.25f, 0.5f, 1.0f, 2.0f, 4.0f, 8.0f };

	private void Start () {
        _slider = GetComponent<Slider>();

        if (_slider.maxValue - _slider.minValue + 1 != valueMap.Length)
        {
            throw new System.Exception("Slider must have the same amount of values as valueMap");
        }

        _slider.onValueChanged.AddListener(OnValueChanged);
	}
	
	private void Update () {
		
	}

    public void OnValueChanged(float value)
    {
        var timescale = valueMap[(int)value];

        PlaybackManager.Instance.SetTimescale(timescale);

        _text.text = timescale.ToString();
    }
}
