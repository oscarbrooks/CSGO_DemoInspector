using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIShadow : MonoBehaviour {

    public float opacity = 1;
    public float distance = 1;

    private void Start () {
        AddOutline(0.3f * distance, 0.5f * opacity);
        AddOutline(0.7f * distance, 0.25f * opacity);
        AddOutline(1.0f * distance, 0.1f * opacity);
    }
	
	private void Update () {
		
	}

    private Outline AddOutline(float distance, float alpha)
    {
        var outline = gameObject.AddComponent<Outline>();

        outline.effectDistance = Vector2.one * distance;

        outline.effectColor = Outline(alpha);

        return outline;
    }

    private Color Outline(float alpha)
    {
        var color = Color.black;
        color.a = alpha;

        return color;
    }
}
