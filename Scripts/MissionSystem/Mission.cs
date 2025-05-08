using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Mission")]
public class Mission : ScriptableObject {
    public Item[] items;
    public float reward;
}
