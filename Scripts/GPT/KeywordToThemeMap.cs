using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeywordToThemeMap", menuName = "AI/Keyword To Theme Map")]
public class KeywordToThemeMap : ScriptableObject
{
    [System.Serializable]
    public class KeywordEntry
    {
        public string keyword;
        public string theme;
    }

    public List<KeywordEntry> keywordEntries;

    public string GetThemeForKeyword(string word)
    {
        foreach (var entry in keywordEntries)
        {
            if (entry.keyword.Equals(word.ToLower()))
                return entry.theme;
        }
        return null;
    }
}
