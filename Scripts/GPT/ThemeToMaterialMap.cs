using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeToMaterialMap", menuName = "AI/Theme To Material Map")]
public class ThemeToMaterialMap : ScriptableObject
{
    [System.Serializable]
    public class ThemeEntry
    {
        public string theme;
        public List<string> materials;
    }

    public List<ThemeEntry> themeEntries;

    public List<string> GetMaterialsForTheme(string theme)
    {
        foreach (var entry in themeEntries)
        {
            if (entry.theme.Equals(theme.ToLower()))
                return entry.materials;
        }
        return new List<string>();
    }
}
