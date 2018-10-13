using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Radio Buttons Controller")]
public class RadioButtonsController : MonoBehaviour {

    [SerializeField]
    private ButtonSelectionEffect[] _buttons;

	private void Start () {
        foreach (var button in _buttons)
        {
            button.AddListener(() => UpdateSelection(button));
        }
	}
	
	private void Update () {
		
	}

    private void UpdateSelection(ButtonSelectionEffect buttonToSelect)
    {
        foreach (var button in _buttons)
        {
            button.SetSelected(button == buttonToSelect);
        }
    }
}
