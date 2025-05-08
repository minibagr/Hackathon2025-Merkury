using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    public static bool fullscreen;
    public static float volume;
    public static Vector2Int resolution;

    private void Awake() {
        Screen.SetResolution(1920, 1080, true);
    }

    public void Extract() {
        Resolution(resolutionDropdown.options.ElementAt(resolutionDropdown.value).text);

        volume = volumeSlider.value * (100 / volumeSlider.maxValue);

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
        Screen.SetResolution(resolution.x, resolution.y, fullscreen);
    }
}
