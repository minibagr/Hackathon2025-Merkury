using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    public static Inventory inv;

    [SerializeField] private Item[] items;
    [SerializeField] private int index;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text price;
    [SerializeField] private Image img;
    [SerializeField] private Sound sound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip successSound;

    private void Start() {
        UpdateData();
    }

    public void Next() {
        index++;
        if (index == items.Length) index = 0;
        UpdateData();
    }

    public void Previous() {
        index--;
        if (index < 0) index = items.Length - 1;
        UpdateData();
    }

    public void Buy() {
        if (sound.audioSource.isPlaying) return;

        if (Player.inventory.isFull()) {
            sound.audioSource.clip = errorSound;
            sound.PlaySound();
            return;
        }

        Money.state state = Money.UpdateMoney(-items[index].price);

        if (state == Money.state.Fail) {
            sound.audioSource.clip = errorSound;
            sound.PlaySound();
            return;
        }

        if (items[index] is ToolItem toolItem) Player.inventory.AddItem(toolItem.DuplicateTool(), 1);
        else if (items[index] is MaterialItem materialItem) Player.inventory.AddItem(materialItem.DuplicateMaterial(), 1);
        else if (items[index] is ConsumableItem consumableItem) Player.inventory.AddItem(consumableItem.DuplicateConsumable(), 1);
        else Player.inventory.AddItem(items[index].Duplicate(items[index]), 1);

        sound.audioSource.clip = successSound;
        sound.PlaySound();
    }

    private void UpdateData() {
        itemName.text = items[index].itemName;
        price.text = items[index].price.ToString();
        img.sprite = items[index].icon;
    }
}
