using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField] private Item[] items;
    [SerializeField] private int index;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text price;
    [SerializeField] private Image img;
    [SerializeField] private Money money;

    private void Awake() {
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
        //Money.state state = money.UpdateMoney(-items[index].price);
        Money.state state = money.UpdateMoney(-25);

        if (state == Money.state.Fail) return;

        Debug.Log("buy");
        // TODO 1: Add item
    }

    private void UpdateData() {
        itemName.text = items[index].itemName;
        //price.text = items[index].price;
        price.text = "25";
        img.sprite = items[index].icon;
    }
}
