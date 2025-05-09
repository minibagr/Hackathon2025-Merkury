using UnityEngine;

[CreateAssetMenu(fileName = "MeshSettings")]
public class MeshSettings : ScriptableObject
{
    public const int numSupportedLODs = 5;
    public const int numSupportedChunkSizes = 9;
    public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

    public float meshScale = 2f;

    [Range(0, numSupportedChunkSizes - 1)]
    public int chunkSizeIndex;

    // num verts per line of mesh rendered at LOD = 0. Includes the 2 extra verts that a re excluded from final mesh, but used for calculation normals
    public int numVertsPerLine
    {
        get
        {
            return supportedChunkSizes[chunkSizeIndex] + 1;
        }
    }

    public float meshWorldSize
    {
        get
        {
            return (numVertsPerLine - 3) * meshScale;
        }
    }
}
