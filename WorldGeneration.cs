using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public GameObject blockGameobject;
    public GameObject objectToSpawn;

    private int worldSizeX = 40;
    private int worldSizeZ = 40;
    private int noiseHeight = 3;
    private float gridOffset = 1.1f;
    private List<Vector3> blockPositions = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int x = 0; x < worldSizeX; x++)
        {
            for(int z = 0; z < worldSizeZ; z++)
            {
                Vector3 pos = new Vector3(x * gridOffset, generateNoise(x, z, 4f) * noiseHeight, z * gridOffset);
                GameObject block = Instantiate(blockGameobject, pos, Quaternion.identity);

                blockPositions.Add(block.transform.position);

                block.transform.parent = this.transform; 
            }
        }
        SpawnObject();
    }

    private void SpawnObject()
    {
        for(int c = 0; c < 35; c++)
        {
            Vector3 spawnPos = ObjectSpawnLocation();
            GameObject obj = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
            obj.transform.parent = this.transform;
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockPositions.Count);

        Vector3 newPos = new Vector3(
            blockPositions[rndIndex].x,
            blockPositions[rndIndex].y + .5f,
            blockPositions[rndIndex].z
        );

        blockPositions.RemoveAt(rndIndex);

        return newPos;
    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.y) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}
