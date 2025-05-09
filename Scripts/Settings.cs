using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider volumeSlider; // Visuals Done // Missing Functionality
    [SerializeField] private Slider fpsSlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    public static bool fullscreen;
    public static float volume;
    public static int fps;
    public static float mouseSensitivity;
    public static float fov;
    public static Vector2Int resolution;

    public void Extract() {
        Resolution(resolutionDropdown.options.ElementAt(resolutionDropdown.value).text);

        volume = volumeSlider.value * (100 / volumeSlider.maxValue);

        fps = (int)fpsSlider.value;

        mouseSensitivity = mouseSensitivitySlider.value;

        fov = fovSlider.value;

        fullscreen = fullscreenToggle.isOn;

        Apply();
    }

    private static readonly Dictionary<string, Vector2Int> ResolutionMap = new Dictionary<string, Vector2Int> {
        { "2560x1440", new Vector2Int(2560, 1440) },
        { "1920x1080", new Vector2Int(1920, 1080) },
        { "1366x768",  new Vector2Int(1366, 768) },
        { "1280x720",  new Vector2Int(1280, 720) }
    };

    private void Resolution(string value) {
        if (ResolutionMap.ContainsKey(value)) resolution = ResolutionMap[value];
        else resolution = new Vector2Int(1920, 1080);
    }

    public void Apply() {
        QualitySettings.vSyncCount = 0;
        if (fps == 121) Application.targetFrameRate = -1;
        else Application.targetFrameRate = fps;

        Player.mouseSensitivity = mouseSensitivity;

        Camera.main.fieldOfView = fov;

        Screen.SetResolution(resolution.x, resolution.y, fullscreen);
    }
}
