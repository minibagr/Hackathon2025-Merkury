using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Button button;
    public TMP_InputField inputField;
    public PromptProcessor promptProcessor;
    public GameObject camera;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        string inputText = inputField.text;
        // Process the input text
        promptProcessor.ProcessPrompt(inputText);
        camera.SetActive(false);
        gameObject.SetActive(false);
    }
}
