using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Added for LINQ usage
using System; // Added for DateTime and Environment

// Attach this script to an empty GameObject in your Unity scene.
// This script will generate the terrain and place objects based on parameters.
public class SceneGenerator : MonoBehaviour
{
    [Header("Terrain Settings")]
    public int terrainGridSize = 200; // Increased size of the terrain grid (e.g., 200x200)
    public float terrainScale = 15f; // Adjusted scale for larger terrain
    public float terrainHeightMultiplier = 8f; // Adjusted multiplier for height
    public Material terrainMaterial; // Material to apply to the terrain mesh

    [Header("Spawn Area Settings")]
    public int spawnAreaSize = 20; // Increased size of the flat square spawn area (e.g., 20x20 tiles)
    public float spawnAreaHeight = 0f; // The fixed height of the flat spawn area

    [Header("Object Placement Settings")]
    public GameObject environmentalObjectPrefab; // Prefab for environmental objects (e.g., trees, rocks)
    [Range(0f, 1f)]
    public float environmentalObjectDensity = 0.1f; // Density of environmental objects (0 to 1)

    [Header("Resource Node Settings")]
    public GameObject woodNodePrefab; // Prefab for wood resource nodes
    public GameObject diamondNodePrefab; // Prefab for diamond resource nodes
    [Range(0f, 1f)]
    public float woodNodeProbability = 0.05f; // Probability of placing a wood node (0 to 1)
    [Range(0f, 1f)]
    public float diamondNodeProbability = 0.02f; // Probability of placing a diamond node (0 to 1)

    // --- Simulation of Receiving AI Output Parameters ---
    // In your actual game, these parameters would come from your AI/rule-based system
    public enum EnvironmentType { Forest, Cave, Desert, Icy, Volcanic, Mountainous, Unknown } // Added Mountainous
    // Removed the hardcoded currentEnvironmentType and currentResourceProbabilities
    // These will now be determined by the text prompt.

    [System.Serializable]
    public class ResourceProbabilities
    {
        [Range(0f, 1f)] public float wood = 0.0f; // Start with 0 probability
        [Range(0f, 1f)] public float diamond = 0.0f; // Start with 0 probability
    }
    // Removed the hardcoded instance of ResourceProbabilities

    // Start is now empty, scene generation will be triggered by calling GenerateSceneFromPrompt
    void Start()
    {
        // Scene generation is now triggered by the player's text prompt
        // You would call GenerateSceneFromPrompt from your UI or game manager script
    }

    /// <summary>
    /// Generates the game scene based on a player's text prompt.
    /// This method would integrate with your AI/rule-based system.
    /// </summary>
    /// <param name="playerPrompt">The text input from the player.</param>
    public void GenerateSceneFromPrompt(string playerPrompt)
    {
        Debug.Log("Received prompt: " + playerPrompt);

        // --- Initialize Random Seed for Unique Generation ---
        // Using the current time ensures a different seed each time this method is called.
        UnityEngine.Random.InitState(System.Environment.TickCount);
        // Or you could use: UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

        // --- Rule-Based Prompt Interpretation ---
        // This is a more detailed example of how you might interpret the prompt
        // using keywords and phrases. This replaces the previous TODO section.

        EnvironmentType determinedEnvironmentType = EnvironmentType.Unknown;
        ResourceProbabilities determinedResourceProbabilities = new ResourceProbabilities(); // Default probabilities

        string lowerPrompt = playerPrompt.ToLower();

        // --- Determine Environment Type ---
        // Check for keywords related to environment types. This primarily sets the visual theme.
        if (lowerPrompt.Contains("forest") || lowerPrompt.Contains("trees") || lowerPrompt.Contains("nature") || lowerPrompt.Contains("jungle"))
        {
            determinedEnvironmentType = EnvironmentType.Forest;
        }
        else if (lowerPrompt.Contains("cave") || lowerPrompt.Contains("underground") || lowerPrompt.Contains("cavern"))
        {
            determinedEnvironmentType = EnvironmentType.Cave;
        }
        else if (lowerPrompt.Contains("desert") || lowerPrompt.Contains("sand") || lowerPrompt.Contains("arid"))
        {
            determinedEnvironmentType = EnvironmentType.Desert;
        }
        else if (lowerPrompt.Contains("ice") || lowerPrompt.Contains("snow") || lowerPrompt.Contains("frozen"))
        {
            determinedEnvironmentType = EnvironmentType.Icy;
        }
        else if (lowerPrompt.Contains("volcano") || lowerPrompt.Contains("lava") || lowerPrompt.Contains("fiery"))
        {
            determinedEnvironmentType = EnvironmentType.Volcanic;
        }
        else if (lowerPrompt.Contains("mountain") || lowerPrompt.Contains("peak") || lowerPrompt.Contains("hill"))
        {
            determinedEnvironmentType = EnvironmentType.Mountainous;
        }
        // If no specific environment is mentioned, you could default to a "mixed" or "temperate" type
        if (determinedEnvironmentType == EnvironmentType.Unknown)
        {
            determinedEnvironmentType = EnvironmentType.Forest; // Default to forest if none specified
            Debug.Log("No specific environment detected, defaulting to Forest.");
        }


        // --- Determine Resource Probabilities (Primarily based on resource-specific keywords) ---
        // Increase probabilities based on keywords and phrases, avoiding "wood" and "diamond".
        // The environment type will influence *where* they are placed, but not necessarily *if* they appear.

        // Keywords/phrases suggesting Wood
        if (lowerPrompt.Contains("green things") || lowerPrompt.Contains("lumber") || lowerPrompt.Contains("timber") || lowerPrompt.Contains("foliage") || lowerPrompt.Contains("vegetation"))
        {
            determinedResourceProbabilities.wood = Mathf.Clamp01(determinedResourceProbabilities.wood + 0.6f); // Significant increase if wood terms are used
        }
        // Additive increase if environment is typically woody, but less impactful than explicit keywords
        if (determinedEnvironmentType == EnvironmentType.Forest || determinedEnvironmentType == EnvironmentType.Mountainous)
        {
            determinedResourceProbabilities.wood = Mathf.Clamp01(determinedResourceProbabilities.wood + 0.15f);
        }


        // Keywords/phrases suggesting Diamonds
        if (lowerPrompt.Contains("shiny rocks") || lowerPrompt.Contains("gems") || lowerPrompt.Contains("crystals") || lowerPrompt.Contains("sparkling stones") || lowerPrompt.Contains("minerals"))
        {
            determinedResourceProbabilities.diamond = Mathf.Clamp01(determinedResourceProbabilities.diamond + 0.7f); // Significant increase if diamond terms are used
        }
        // Additive increase if environment is typically mineral-rich, but less impactful than explicit keywords
        if (determinedEnvironmentType == EnvironmentType.Cave || determinedEnvironmentType == EnvironmentType.Volcanic || determinedEnvironmentType == EnvironmentType.Mountainous)
        {
            determinedResourceProbabilities.diamond = Mathf.Clamp01(determinedResourceProbabilities.diamond + 0.2f);
        }


        // --- Adjust probabilities based on combinations or specific requests ---
        // If the prompt explicitly mentions terms for both resources, ensure both are likely
        if ((lowerPrompt.Contains("green things") || lowerPrompt.Contains("lumber")) && (lowerPrompt.Contains("shiny rocks") || lowerPrompt.Contains("gems")))
        {
            determinedResourceProbabilities.wood = Mathf.Max(determinedResourceProbabilities.wood, 0.5f); // Ensure at least moderate wood if explicitly asked for
            determinedResourceProbabilities.diamond = Mathf.Max(determinedResourceProbabilities.diamond, 0.5f); // Ensure at least moderate diamond if explicitly asked for
        }


        // --- Apply Special Attributes (Simple Example) ---
        // You would expand this to handle different attributes and their effects
        // For now, just logging if a key attribute is mentioned.
        if (lowerPrompt.Contains("hot") || lowerPrompt.Contains("scorching"))
        {
            Debug.Log("Attribute detected: Hot");
            // TODO: Implement gameplay effect for 'Hot'
        }
        if (lowerPrompt.Contains("gassy") || lowerPrompt.Contains("toxic"))
        {
            Debug.Log("Attribute detected: Gassy");
            // TODO: Implement gameplay effect for 'Gassy'
        }


        // --- Call the Scene Generation ---
        // Always generate a scene if an environment type was determined (even if defaulted)
        // The resource probabilities will now be more directly tied to the prompt's keywords.
        Debug.Log("Interpreted as: " + determinedEnvironmentType + " with Wood Prob: " + determinedResourceProbabilities.wood + ", Diamond Prob: " + determinedResourceProbabilities.diamond);
        GenerateScene(determinedEnvironmentType, determinedResourceProbabilities);

    }


    /// <summary>
    /// Generates the game scene based on the provided parameters.
    /// </summary>
    /// <param name="environmentType">The type of environment to generate.</param>
    /// <param name="resourceProbabilities">The probabilities for placing different resources.</param>
    public void GenerateScene(EnvironmentType environmentType, ResourceProbabilities resourceProbabilities)
    {
        // Clear any previously generated scene elements
        ClearScene();

        // --- Step 2: Generate Terrain ---
        Debug.Log("Generating terrain...");
        // You might adjust terrain generation parameters based on environmentType here
        float currentTerrainScale = terrainScale;
        float currentHeightMultiplier = terrainHeightMultiplier;

        if (environmentType == EnvironmentType.Mountainous) // Example of adjusting based on type
        {
            currentHeightMultiplier *= 2.0f;
            currentTerrainScale *= 0.8f;
        }
        else if (environmentType == EnvironmentType.Cave)
        {
            currentHeightMultiplier *= 0.5f; // Caves might have less vertical variation overall
        }
        // Add more adjustments for other environment types


        Mesh terrainMesh = GenerateTerrainMesh(terrainGridSize, currentTerrainScale, currentHeightMultiplier, spawnAreaSize, spawnAreaHeight);
        GameObject terrainObject = new GameObject("Terrain");
        terrainObject.transform.parent = this.transform; // Parent to this generator object

        MeshFilter meshFilter = terrainObject.AddComponent<MeshFilter>();
        meshFilter.mesh = terrainMesh;

        MeshRenderer meshRenderer = terrainObject.AddComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial; // Assign the material

        // Add a collider so the player can walk on it
        terrainObject.AddComponent<MeshCollider>();
        Debug.Log("Terrain generated.");

        // --- Step 3 & 4: Place Environmental Objects and Resource Nodes ---
        Debug.Log("Placing objects and resource nodes...");
        PlaceObjectsAndResources(terrainMesh, environmentType, resourceProbabilities, spawnAreaSize);
        Debug.Log("Objects and resource nodes placed.");

        // --- Step 5: Apply Scene Attributes (Simple Simulation) ---
        Debug.Log("Applying scene attributes...");
        ApplySceneAttributes(environmentType);
        Debug.Log("Scene attributes applied.");
    }

    /// <summary>
    /// Generates a terrain mesh using Perlin noise with a flat spawn area in the center.
    /// </summary>
    private Mesh GenerateTerrainMesh(int size, float scale, float heightMultiplier, int spawnSize, float spawnHeight)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[size * size];
        int[] triangles = new int[(size - 1) * (size - 1) * 6];
        Vector2[] uv = new Vector2[size * size];

        // Use a random offset for different terrains shapes each time
        float offsetX = UnityEngine.Random.Range(0f, 1000f);
        float offsetY = UnityEngine.Random.Range(0f, 1000f);

        // Calculate the center coordinates
        int centerX = size / 2;
        int centerZ = size / 2;

        // Calculate the bounds of the flat spawn area
        int spawnStartX = centerX - spawnSize / 2;
        int spawnEndX = centerX + spawnSize / 2;
        int spawnStartZ = centerZ - spawnSize / 2;
        int spawnEndZ = centerZ + spawnSize / 2;

        // Generate vertices and UVs
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float height;

                // Check if the current vertex is within the flat spawn area
                if (x >= spawnStartX && x < spawnEndX && z >= spawnStartZ && z < spawnEndZ)
                {
                    height = spawnHeight; // Set height to the fixed spawn height
                }
                else
                {
                    // Generate height from Perlin noise for areas outside the spawn
                    float sampleX = (float)x / size * scale + offsetX;
                    float sampleZ = (float)z / size * scale + offsetY;
                    height = Mathf.PerlinNoise(sampleX, sampleZ) * heightMultiplier;

                    // --- Optional: Smooth transition between flat and noisy terrain ---
                    // You can blend the noise towards the edges of the spawn area
                    float blendDistance = 5f; // How far from the spawn edge to blend
                    float distToSpawnEdge = Mathf.Min(
                        Mathf.Abs(x - spawnStartX), Mathf.Abs(x - spawnEndX),
                        Mathf.Abs(z - spawnStartZ), Mathf.Abs(z - spawnEndZ)
                    );

                    if (distToSpawnEdge < blendDistance)
                    {
                        float blendFactor = distToSpawnEdge / blendDistance; // 0 at edge, 1 at blendDistance away
                        // Interpolate between spawnHeight and noise height
                        height = Mathf.Lerp(spawnHeight, height, blendFactor);
                    }
                    // --- End Optional Smoothing ---
                }


                vertices[z * size + x] = new Vector3(x, height, z);
                uv[z * size + x] = new Vector2((float)x / (size - 1), (float)z / (size - 1));
            }
        }

        // Generate triangles
        int vert = 0;
        int tri = 0;
        for (int z = 0; z < size - 1; z++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                int v0 = vert;
                int v1 = vert + size;
                int v2 = vert + 1;
                int v3 = vert + size + 1;

                triangles[tri + 0] = v0;
                triangles[tri + 1] = v1;
                triangles[tri + 2] = v2;
                triangles[tri + 3] = v2;
                triangles[tri + 4] = v1;
                triangles[tri + 5] = v3;

                vert++;
                tri += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals(); // Needed for lighting
        return mesh;
    }

    /// <summary>
    /// Places environmental objects and resource nodes on the terrain, avoiding the spawn area.
    /// </summary>
    private void PlaceObjectsAndResources(Mesh terrainMesh, EnvironmentType environmentType, ResourceProbabilities resourceProbabilities, int spawnSize)
    {
        Vector3[] vertices = terrainMesh.vertices;
        int size = (int)Mathf.Sqrt(vertices.Length); // Assuming square grid

        // Calculate the bounds of the flat spawn area
        int centerX = size / 2;
        int centerZ = size / 2;
        int spawnStartX = centerX - spawnSize / 2;
        int spawnEndX = centerX + spawnSize / 2;
        int spawnStartZ = centerZ - spawnSize / 2;
        int spawnEndZ = centerZ + spawnSize / 2;


        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                // Check if the current position is within the flat spawn area
                bool inSpawnArea = x >= spawnStartX && x < spawnEndX && z >= spawnStartZ && z < spawnEndZ;

                Vector3 vertexPosition = vertices[z * size + x];
                // Adjust position to be in world space if terrain object is not at origin
                Vector3 worldPosition = transform.TransformPoint(vertexPosition);

                // Simple check to avoid placing objects too close to the edge of the *entire terrain*
                if (x > 1 && x < size - 1 && z > 1 && z < size - 1)
                {
                    // --- Place Environmental Objects ---
                    // Only place if NOT in the spawn area
                    if (!inSpawnArea)
                    {
                        // You would select different prefabs based on environmentType here
                        // and potentially adjust density based on environment type.
                        float currentEnvironmentalObjectDensity = environmentalObjectDensity;
                        if (environmentType == EnvironmentType.Desert)
                        {
                            currentEnvironmentalObjectDensity *= 0.5f; // Less objects in desert
                        }
                        else if (environmentType == EnvironmentType.Forest)
                        {
                            currentEnvironmentalObjectDensity *= 1.5f; // More objects in forest
                        }
                        // Add more adjustments for other environment types


                        if (UnityEngine.Random.value < currentEnvironmentalObjectDensity) // Use UnityEngine.Random
                        {
                            // TODO: Select different environmental prefabs based on environmentType
                            if (environmentalObjectPrefab != null)
                            {
                                GameObject obj = Instantiate(environmentalObjectPrefab, worldPosition, Quaternion.identity);
                                obj.transform.parent = this.transform; // Parent for organization
                                // Randomize rotation slightly
                                obj.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0); // Use UnityEngine.Random
                                // You might adjust the y-position slightly to sit on the surface
                                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
                            }
                        }
                    }

                    // --- Place Resource Nodes ---
                    // Only place if NOT in the spawn area
                    if (!inSpawnArea)
                    {
                        // Wood Node Placement (Example based on height/environment and probability)
                        // Place wood nodes primarily in lower/mid-height areas or areas suitable for trees
                        bool canPlaceWood = false;
                        // Placement rules are still influenced by environment, but probability is from prompt
                        if (environmentType == EnvironmentType.Forest || environmentType == EnvironmentType.Mountainous)
                        {
                            if (worldPosition.y < terrainHeightMultiplier * 0.7f) // Wood on lower/mid slopes in forests/mountains
                            {
                                canPlaceWood = true;
                            }
                        }
                        // Allow wood to be placed in other environments if the probability is high enough from the prompt
                        if (resourceProbabilities.wood > 0.6f && environmentType != EnvironmentType.Forest && environmentType != EnvironmentType.Mountainous)
                        {
                            canPlaceWood = true; // Can place wood even in non-typical environments if prompt strongly suggested it
                        }


                        if (canPlaceWood && UnityEngine.Random.value < resourceProbabilities.wood) // Use UnityEngine.Random
                        {
                            if (woodNodePrefab != null)
                            {
                                GameObject woodNode = Instantiate(woodNodePrefab, worldPosition, Quaternion.identity);
                                woodNode.transform.parent = this.transform; // Parent for organization
                                // Add the ResourceNode component to handle interaction
                                if (woodNode.GetComponent<ResourceNode>() == null)
                                {
                                    woodNode.AddComponent<ResourceNode>().resourceType = "Wood";
                                }
                                // Adjust y-position slightly to sit on the surface
                                woodNode.transform.position = new Vector3(woodNode.transform.position.x, woodNode.transform.position.y, woodNode.transform.position.z);
                            }
                        }

                        // Diamond Node Placement (Example based on height/environment and probability)
                        // Place diamond nodes primarily in higher/rockier areas, caves, or volcanic areas
                        bool canPlaceDiamond = false;
                        // Placement rules are still influenced by environment, but probability is from prompt
                        if (environmentType == EnvironmentType.Cave || environmentType == EnvironmentType.Volcanic || environmentType == EnvironmentType.Mountainous)
                        {
                            if (worldPosition.y > terrainHeightMultiplier * 0.3f) // Diamonds in higher/rockier areas
                            {
                                canPlaceDiamond = true;
                            }
                        }
                        // Allow diamond to be placed in other environments if the probability is high enough from the prompt
                        if (resourceProbabilities.diamond > 0.6f && environmentType != EnvironmentType.Cave && environmentType != EnvironmentType.Volcanic && environmentType != EnvironmentType.Mountainous)
                        {
                            canPlaceDiamond = true; // Can place diamond even in non-typical environments if prompt strongly suggested it
                        }


                        if (canPlaceDiamond && UnityEngine.Random.value < resourceProbabilities.diamond) // Use UnityEngine.Random
                        {
                            if (diamondNodePrefab != null)
                            {
                                GameObject diamondNode = Instantiate(diamondNodePrefab, worldPosition, Quaternion.identity);
                                diamondNode.transform.parent = this.transform; // Parent for organization
                                // Add the ResourceNode component to handle interaction
                                if (diamondNode.GetComponent<ResourceNode>() == null)
                                {
                                    diamondNode.AddComponent<ResourceNode>().resourceType = "Diamond";
                                }
                                // Adjust y-position slightly to sit on the surface
                                diamondNode.transform.position = new Vector3(diamondNode.transform.position.x, diamondNode.transform.position.y, diamondNode.transform.position.z);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Applies scene-wide attributes based on the environment type (simple simulation).
    /// </summary>
    private void ApplySceneAttributes(EnvironmentType environmentType)
    {
        // Example: Change ambient light color based on environment
        switch (environmentType)
        {
            case EnvironmentType.Forest:
                RenderSettings.ambientLight = new Color(0.4f, 0.5f, 0.4f); // Greenish ambient light
                break;
            case EnvironmentType.Desert:
                RenderSettings.ambientLight = new Color(0.6f, 0.5f, 0.3f); // Orangish ambient light
                break;
            case EnvironmentType.Cave:
                RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.3f); // Darker, bluish ambient light
                break;
            case EnvironmentType.Icy:
                RenderSettings.ambientLight = new Color(0.5f, 0.6f, 0.7f); // Bluish-white ambient light
                break;
            case EnvironmentType.Volcanic:
                RenderSettings.ambientLight = new Color(0.7f, 0.4f, 0.2f); // Reddish-orange ambient light
                break;
            case EnvironmentType.Mountainous:
                RenderSettings.ambientLight = new Color(0.4f, 0.4f, 0.4f); // Grayish ambient light
                break;
            default:
                RenderSettings.ambientLight = Color.gray; // Default
                break;
        }

        // TODO: Add logic here to:
        // - Change skybox based on environmentType
        // - Add particle effects (e.g., dust in desert, snow in icy, steam in volcanic)
        // - Enable/disable specific gameplay systems (e.g., temperature management, gas mask requirement)
    }

    /// <summary>
    /// Clears all previously generated objects parented to this generator.
    /// </summary>
    private void ClearScene()
    {
        // Destroy all child objects of this generator
        while (transform.childCount > 0)
        {
            // Use DestroyImmediate in editor scripts or when setting up the scene
            // In runtime gameplay, use Destroy() instead.
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
