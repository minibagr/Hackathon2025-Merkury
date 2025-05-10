using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PromptProcessor : MonoBehaviour
{
    public BiomeDatabase biomeDatabase;
    public MaterialDatabase materialDatabase;
    public TerrainGenerator terrainGenerator;

    private HashSet<string> bannedWords = new HashSet<string>();

    public void ProcessPrompt(string inputPrompt)
    {
        string[] words = inputPrompt.ToLower().Split(' ', '.', ',', '!', '?');
        List<string> validWords = words.Where(word => !bannedWords.Contains(word) && word.Length > 2).ToList();

        // Ban the words for next time
        foreach (string word in validWords)
            bannedWords.Add(word);

        Biome selectedBiome = biomeDatabase.MatchBiome(validWords);
        List<MaterialType> selectedMaterials = materialDatabase.MatchMaterials(validWords);
        int bugCount = Random.Range(1, 4); // simulate error/misinterpretation

        terrainGenerator.CreateTerrain(selectedBiome, selectedMaterials);
    }
}

[System.Serializable]
public class ProcessedPromptData
{
    public Biome biome;
    public List<MaterialType> materials;
    public int bugCount;
}
