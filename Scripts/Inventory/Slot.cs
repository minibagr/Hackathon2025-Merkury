using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    public Item itemInSlot;
    public Image itemIconImage;
    public TMP_Text amountText;
    public int amount;

    [HideInInspector] public Image borderImage;

    void Start() {borderImage = gameObject.transform.GetChild(0).GetComponent<Image>();}

    public void UpdateUI() {
        itemIconImage.sprite = itemInSlot.icon;
        if (itemInSlot.maxStack != 1) amountText.text = amount.ToString();
        else amountText.text = "";
    }
}
