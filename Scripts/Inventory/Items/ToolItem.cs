using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool Item")]
public class ToolItem : Item {
    public GameObject toolPrefab;
    public MaterialItem.MaterialType toolType;
    public float durability;
    public float damage;
    public float range;
}
