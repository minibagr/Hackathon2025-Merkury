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
}
