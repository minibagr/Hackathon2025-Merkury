using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickScript : MonoBehaviour
{
    public Button button;
    public TMP_InputField textField;
    public EnvironmentGenerator sceneGenerator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Debug.Log(textField.text);
        sceneGenerator.GenerateWorldFromPrompt(textField.text);
    }
}
