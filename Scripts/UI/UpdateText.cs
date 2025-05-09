using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour {
    [SerializeField] private TMP_Text text;
    [SerializeField] private Slider slider;
    [SerializeField] private string condition;
    [SerializeField] private string replaceWith;

    public void ForceUpdateText() {
        string value = slider.value.ToString();

        if (value == condition) value = replaceWith;

        text.text = value;
    }
}
