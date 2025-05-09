using UnityEngine.UI;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Item itemInSlot;
    public Image itemIconImage;
    
    [HideInInspector] public Image borderImage;

    void Start() {borderImage = gameObject.transform.GetChild(0).GetComponent<Image>();}

    public void UpdateUI() {
        itemIconImage.sprite = itemInSlot.icon;
    }
}
