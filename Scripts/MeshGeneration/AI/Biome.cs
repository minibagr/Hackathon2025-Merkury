
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Biome", menuName = "NLP/Biome")]
public class Biome : ScriptableObject
{
    public string biomeName;
    public HeightMapSettings heightMapSettings;
    public MeshSettings meshSettings;
    public TextureData textureData;
    public List<string> keywords;
}