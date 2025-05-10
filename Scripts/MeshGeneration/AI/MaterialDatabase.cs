using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialDatabase", menuName = "NLP/MaterialDatabase")]
public class MaterialDatabase : ScriptableObject
{
    public List<MaterialType> materials;

    public List<MaterialType> MatchMaterials(List<string> words)
    {
        List<MaterialType> results = new List<MaterialType>();
        foreach (var mat in materials)
        {
            if (mat.associatedKeywords.Exists(k => words.Contains(k.ToLower())))
            {
                results.Add(mat);
            }
        }

        return results;
    }
}

[System.Serializable]
public class MaterialType
{
    public string materialName;
    public GameObject prefab;
    public List<string> associatedKeywords;
}
