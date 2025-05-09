using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable Item")]
public class ConsumableItem : Item {
    public int healAmount;
    public AudioClip useSound;
}