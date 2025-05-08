using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    [Header("Script References")]
    public PromptProcessor promptProcessor;

    [Header("Generation Settings")]
    public List<BiomeDefinition> biomeDefinitions;
    public List<GameObject> materialNodes;
    public Transform worldParent;

    [Header("Map Size Settings")]
    public int mapSize = 10;            // Number of chunks in X/Z direction
    public float chunkSize = 10f;       // Size of each chunk

    private Dictionary<string, BiomeDefinition> biomeMap = new();

    void Start()
    {
        // Build dictionary for faster lookup
        foreach (var biome in biomeDefinitions)
        {
            biomeMap[biome.theme.ToLower()] = biome;
        }
    }

    public void GenerateWorldFromPrompt(string prompt)
    {
        // Clean up previous world
        foreach (Transform child in worldParent)
        {
            Destroy(child.gameObject);
        }

        promptProcessor.ProcessPrompt(prompt);
        var themes = promptProcessor.GetThemes();
        var materials = promptProcessor.GetMaterials();

        // Safety check: use first matching biome if multiple themes found
        if (themes.Count == 0)
        {
            Debug.LogWarning("No themes found for prompt.");
            return;
        }

        string primaryTheme = themes[0];
        if (!biomeMap.TryGetValue(primaryTheme.ToLower(), out BiomeDefinition biome))
        {
            Debug.LogWarning("No biome found for theme: " + primaryTheme);
            return;
        }

        // Spawn grid of terrain chunks
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Vector3 chunkPos = new Vector3(x * chunkSize, 0, z * chunkSize);
                GameObject chunk = Instantiate(biome.terrainPrefab, chunkPos, Quaternion.identity, worldParent);

                // Add random decor to each chunk
                foreach (var decor in biome.decorObjects)
                {
                    int decorCount = Random.Range(1, 4);
                    for (int i = 0; i < decorCount; i++)
                    {
                        Vector3 decorPos = chunkPos + new Vector3(Random.Range(0, chunkSize), 0, Random.Range(0, chunkSize));
                        Instantiate(decor, decorPos, Quaternion.identity, worldParent);
                    }
                }
            }
        }

        // Spawn material resource nodes
        foreach (var mat in materials)
        {
            GameObject nodePrefab = materialNodes.Find(p => p.name.ToLower().Contains(mat.ToLower()));
            if (nodePrefab != null)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector3 pos = new Vector3(
                        Random.Range(0, mapSize * chunkSize),
                        0.5f,
                        Random.Range(0, mapSize * chunkSize)
                    );
                    Instantiate(nodePrefab, pos, Quaternion.identity, worldParent);
                }
            }
            else
            {
                Debug.LogWarning("No material prefab found for: " + mat);
            }
        }

        Debug.Log("World generation complete based on prompt: " + prompt);
    }
}
