using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BiomeDefinition", menuName = "AI/Biome Definition")]
public class BiomeDefinition : ScriptableObject
{
    public string theme;
    public GameObject terrainPrefab;
    public List<GameObject> decorObjects;
}
