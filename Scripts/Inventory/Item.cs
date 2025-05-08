using UnityEngine;

public enum ItemType
{
    Tool,
    Consumable,
    Material
}

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
}

[CreateAssetMenu(menuName = "Items/Tool Item")]
public class ToolItem : Item
{
    public GameObject toolPrefab; 
    public int durability;
}

[CreateAssetMenu(menuName = "Items/Consumable Item")]
public class ConsumableItem : Item
{
    public int healAmount;
    public AudioClip useSound;
}

[CreateAssetMenu(menuName = "Items/Material Item")]
public class MaterialItem : Item
{
    public int sellPrice;
}