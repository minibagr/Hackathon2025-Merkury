using UnityEngine;

public class Item : ScriptableObject {
    public enum ItemType {
        Tool,
        Consumable,
        Material
    }

    public string itemName;
    public int price;
    public int maxStack;
    public Sprite icon;
    public ItemType itemType;
    public GameObject itemDrop;

    public Item Duplicate(Item duplicate) {
        duplicate.itemName = itemName;
        duplicate.price = price;
        duplicate.maxStack = maxStack;
        duplicate.icon = icon;
        duplicate.itemType = itemType;
        duplicate.itemDrop = itemDrop;

        return duplicate;
    }
}
