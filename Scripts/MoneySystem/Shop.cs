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
        if (Player.inventory.isFull()) return;

        Money.state state = Money.UpdateMoney(-items[index].price);

        if (state == Money.state.Fail) return;

        Player.inventory.AddItem(items[index], 1);
    }

    private void UpdateData() {
        itemName.text = items[index].itemName;
        price.text = items[index].price.ToString();
        img.sprite = items[index].icon;
    }
}
