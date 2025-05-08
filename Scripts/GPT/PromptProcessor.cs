using System.Collections.Generic;
using UnityEngine;

public class PromptProcessor : MonoBehaviour
{
    public KeywordToThemeMap keywordMap;
    public ThemeToMaterialMap materialMap;

    private HashSet<string> themesFound = new();
    private HashSet<string> materialsFound = new();

    public void ProcessPrompt(string prompt)
    {
        themesFound.Clear();
        materialsFound.Clear();

        string[] words = prompt.ToLower().Split(' ', '.', ',', '!', '?');

        foreach (string word in words)
        {
            string theme = keywordMap.GetThemeForKeyword(word);
            if (!string.IsNullOrEmpty(theme))
            {
                themesFound.Add(theme);
                var materials = materialMap.GetMaterialsForTheme(theme);
                foreach (string mat in materials)
                {
                    materialsFound.Add(mat);
                }
            }
        }

        Debug.Log("Themes: " + string.Join(", ", themesFound));
        Debug.Log("Materials: " + string.Join(", ", materialsFound));
    }

    public List<string> GetThemes() => new(themesFound);
    public List<string> GetMaterials() => new(materialsFound);
}
