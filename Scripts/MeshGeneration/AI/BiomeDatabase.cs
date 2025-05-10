using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeDatabase", menuName = "NLP/BiomeDatabase")]
public class BiomeDatabase : ScriptableObject
{
    public List<Biome> biomes;

    public Biome MatchBiome(List<string> words)
    {
        foreach (var biome in biomes)
        {
            if (biome.keywords.Exists(k => words.Contains(k.ToLower())))
                return biome;
        }

        return biomes[0]; // fallback
    }
}
