using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

public class TerrainGenerator : MonoBehaviour
{

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;


    public int colliderLODIndex;
    public LODInfo[] detailLevels;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureSettings;

    public Transform viewer;
    public Material mapMaterial;

    Vector2 viewerPosition;
    Vector2 viewerPositionOld;

    float meshWorldSize;
    int chunksVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();
    List<MaterialType> materials = new List<MaterialType>();

    TerrainChunk topLeftChunk;
    TerrainChunk topRightChunk;
    TerrainChunk botLeftChunk;
    TerrainChunk botRightChunk;


    public void CreateTerrain(Biome currentBiome, List<MaterialType> materials)
    {
        this.materials = materials;
        heightMapSettings = currentBiome.heightMapSettings;
        meshSettings = currentBiome.meshSettings;
        textureSettings = currentBiome.textureData;
        currentBiome.heightMapSettings.noiseSettings.seed = Random.Range(int.MinValue, int.MaxValue);

        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        float maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        meshWorldSize = meshSettings.meshWorldSize;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);

        Debug.Log("Creating Terrain");

        UpdateVisibleChunks();

        CreateBoundary();
    }

    private void SpawnMaterialsInChunk(TerrainChunk chunk, MeshData meshData)
    {
        Debug.Log("Spawning materials in chunk");

        Vector3[] vertices = meshData.vertices;
        Vector2 position = chunk.coord * chunk.meshSettings.meshWorldSize;

        // Define how many materials we want to spawn
        int count = 50;
        Vector3[] positions = new Vector3[count];
        GameObject[] prefabs = new GameObject[count];

        // Loop to spawn materials based on mesh height
        for (int i = 0; i < count; i++)
        {
            int random = Random.Range(0, vertices.Length);

            positions[i] = new Vector3(position.x + vertices[random].x, vertices[random].y, position.y + vertices[random].z);

            // Select a random prefab material from the materials list
            int prefabIndex = Random.Range(0, materials.Count);
            prefabs[i] = materials[prefabIndex].prefab;
        }

        // Instantiate the selected materials at the calculated positions
        for (int i = 0; i < count; i++)
        {
            GameObject mat = Instantiate(prefabs[i], positions[i], Quaternion.identity, chunk.meshObject.transform);
            mat.transform.position = positions[i];
        }

        if (chunk.coord.x == 0 && chunk.coord.y == 0)
        {
            SpawnPlayerAtCenter(chunk, meshData);
        }
    }

    void SpawnPlayerAtCenter(TerrainChunk chunk, MeshData meshData)
    {
        Vector3[] vertices = meshData.vertices;
        Vector2 position = chunk.coord * chunk.meshSettings.meshWorldSize;
        viewer.parent.gameObject.SetActive(true);
        // Get the center of the terrain
        float centerX = vertices[(vertices.Length - 1) / 2].x;
        float centerY = vertices[(vertices.Length - 1) / 2].z;
        float centerHeight = vertices[(vertices.Length - 1) / 2].y + 2;

        // Create the player at the center
        Vector3 spawnPosition = new Vector3(centerX, centerHeight, centerY);
        viewer.parent.transform.position = spawnPosition;
    }

    void CreateBoundary()
    {
        GameObject boundary = GameObject.Find("Boundary"); // Or create one dynamically
        if (boundary != null)
        {
            BoundaryBox boundaryScript = boundary.GetComponent<BoundaryBox>();
            boundaryScript.player = viewer;
            boundaryScript.boundarySize = meshSettings.meshWorldSize * 4 * meshSettings.meshScale;
            boundary.transform.position = Vector3.zero; // Centered on terrain
        }
    }
    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if (viewerPosition != viewerPositionOld)
        {
            foreach (TerrainChunk chunk in visibleTerrainChunks)
            {
                chunk.UpdateCollisionMesh();
            }
        }

        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks()
    {
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
        for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--)
        {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
            visibleTerrainChunks[i].UpdateTerrainChunk();
        }

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshWorldSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshWorldSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
                {
                    if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    }
                    else
                    {
                        TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial, OnHeightMapLoaded);
                        terrainChunkDictionary.Add(viewedChunkCoord, newChunk);

                        if (viewedChunkCoord.x == 2 && viewedChunkCoord.y == 2)
                        {
                            topRightChunk = newChunk;
                        }
                        if (viewedChunkCoord.x == -2 && viewedChunkCoord.y == 2)
                        {
                            topLeftChunk = newChunk;
                        }
                        if (viewedChunkCoord.x == 2 && viewedChunkCoord.y == -2)
                        {
                            botRightChunk = newChunk;
                        }
                        if (viewedChunkCoord.x == -2 && viewedChunkCoord.y == -2)
                        {
                            botLeftChunk = newChunk;
                        }

                        //newChunk.onHeightMapLoaded += OnHeightMapLoaded;
                        newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                        newChunk.Load();
                    }
                }

            }
        }
    }

    private void OnHeightMapLoaded(TerrainChunk chunk, MeshData lodMeshes)
    {
        SpawnMaterialsInChunk(chunk, lodMeshes); // Call the method once the height map is loaded
    }

    void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
    {
        if (isVisible)
        {
            visibleTerrainChunks.Add(chunk);
        }
        else
        {
            visibleTerrainChunks.Remove(chunk);
        }
    }

}

[System.Serializable]
public struct LODInfo
{
    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int lod;
    public float visibleDstThreshold;


    public float sqrVisibleDstThreshold
    {
        get
        {
            return visibleDstThreshold * visibleDstThreshold;
        }
    }
}