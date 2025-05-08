using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {
    [SerializeField] Slider slider;
    [SerializeField] float scrollModifier = 1;

    public void Slide() {
        transform.localPosition = new Vector3(0, slider.value * scrollModifier, 0);
    }
}
