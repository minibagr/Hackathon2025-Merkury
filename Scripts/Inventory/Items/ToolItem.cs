using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool Item")]
public class ToolItem : Item {
    public GameObject toolPrefab;
    public MaterialItem.MaterialType toolType;
    public float durability;
    public float damage;
    public float range;

    public ToolItem DuplicateTool() {
        ToolItem newItem = CreateInstance<ToolItem>();
        ToolItem duplicate = (ToolItem)Duplicate(newItem);

        duplicate.toolPrefab = toolPrefab;
        duplicate.toolType = toolType;
        duplicate.durability = durability;
        duplicate.damage = damage;
        duplicate.range = range;

        return duplicate;
    }
}
