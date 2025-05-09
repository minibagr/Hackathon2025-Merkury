using UnityEngine;

[CreateAssetMenu(fileName = "NoiseData")]
public class HeightMapSettings : ScriptableObject
{
    public NoiseSettings noiseSettings;

    public bool useFalloff;
    public float heightMultiplier;
    public AnimationCurve heightCurve;

    public float minHeight
    {
        get
        {
            return heightMultiplier * heightCurve.Evaluate(0);
        }
    }

    public float maxHeight
    {
        get
        {
            return heightMultiplier * heightCurve.Evaluate(1);
        }
    }

    private void OnValidate()
    {
        noiseSettings.ValidateValues();
    }
}
