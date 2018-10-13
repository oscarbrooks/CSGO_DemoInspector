using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(InputField))]
public class UIInputSubmitter : MonoBehaviour {

    [SerializeField]
    private InputField _inputField;

    [SerializeField]
    private UnityStringEvent _onSubmit;

	private void Start () {
        _inputField = GetComponent<InputField>();
	}
	
	private void Update () {
        if (_inputField.isFocused && Input.GetKeyDown("return")) _onSubmit.Invoke(_inputField.text);
	}
}
