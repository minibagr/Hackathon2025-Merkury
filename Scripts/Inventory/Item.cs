using UnityEngine;

public class Item : ScriptableObject {
    public enum ItemType {
        Tool,
        Consumable,
        Material
    }

    public string itemName;
    public int price;
    public Sprite icon;
    public ItemType itemType;
}
