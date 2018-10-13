using UnityEngine;
using UnityEngine.UI;

public class DemoViewerUIController : MonoBehaviour {

    [SerializeField]
    private Button _backButton;

	private void Start () {
		_backButton.onClick.AddListener(() => GameManager.Instance.LoadMainMenu());
	}
	
	private void Update () {
		
	}
}
