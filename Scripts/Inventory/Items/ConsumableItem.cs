using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable Item")]
public class ConsumableItem : Item {
    public int healAmount;
    public AudioClip useSound;

    public ConsumableItem DuplicateConsumable() {
        ConsumableItem newItem = CreateInstance<ConsumableItem>();
        ConsumableItem duplicate = (ConsumableItem)Duplicate(newItem);

        duplicate.healAmount = healAmount;
        duplicate.useSound = useSound;

        return duplicate;
    }
}