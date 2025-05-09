using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] inventory;
    public Sprite invinsibleSprite;
    public Item[] randomItem;

    [SerializeField] private Transform itemHolder;

    int selectedSlot = 0; 
    int? previousSelectedSlot = null;

    void UpdateSelectedSlot() {
        for(int i = 0; i < inventory.Length; i++) {
            inventory[i].borderImage.color = new Color32(255,255,255, 255);
        }

        inventory[selectedSlot].borderImage.color = new Color32(255,47,47, 255);

        if(itemHolder.childCount >= 1) {
            Destroy(itemHolder.GetChild(0).gameObject);
        }

        if(inventory[selectedSlot].itemInSlot != null) {
            if(inventory[selectedSlot].itemInSlot.itemType == Item.ItemType.Tool && previousSelectedSlot != selectedSlot) {

                ToolItem itemHolding = (ToolItem) inventory[selectedSlot].itemInSlot;
                GameObject item = Instantiate(itemHolding.toolPrefab, itemHolder.position, itemHolder.rotation);
                item.transform.SetParent(itemHolder.transform);
                item.GetComponent<Tool>().tool = itemHolding;
            }
        }

        if (previousSelectedSlot != selectedSlot) previousSelectedSlot = selectedSlot;
        else {
            inventory[selectedSlot].borderImage.color = new Color32(255, 255, 255, 255);
            previousSelectedSlot = -1;
        }
    }

    void Update() {
        if (Time.timeScale == 0 || !ObjectLock.active) return;

        if(Input.GetKeyDown(KeyCode.Alpha1)) {selectedSlot = 0; UpdateSelectedSlot();}
        if(Input.GetKeyDown(KeyCode.Alpha2)) {selectedSlot = 1; UpdateSelectedSlot();}
        if(Input.GetKeyDown(KeyCode.Alpha3)) {selectedSlot = 2; UpdateSelectedSlot();}
        if(Input.GetKeyDown(KeyCode.Alpha4)) {selectedSlot = 3; UpdateSelectedSlot();}
        if(Input.GetKeyDown(KeyCode.Alpha5)) {selectedSlot = 4; UpdateSelectedSlot();}
    }

    public void AddItem(Item item, int amount) {
        if(isFull()) {return;}
            
        for(int i = 0; i < inventory.Length; i++) {
            if(inventory[i].itemInSlot == null) {
                inventory[i].itemInSlot = item;
                inventory[i].UpdateUI();
                return;
            }
        }
    }

    public void RemoveItem(Item item, int amount) {
        if(!ContainItem(item)) { return;}

        for(int i = inventory.Length - 1; i > -1; i--) {
            if(inventory[i].itemInSlot == item) {
                inventory[i].itemInSlot = null;
                inventory[i].itemIconImage.sprite = invinsibleSprite;
                return;
            }
        }
    }

    public bool isFull() {
        for(int i = 0; i < inventory.Length; i++) {
            if(inventory[i].itemInSlot == null) {
                return false;
            }
        }
        return true;
    }

    public bool ContainItem(Item item) {
        for(int i = 0; i < inventory.Length; i++) {
            if(inventory[i].itemInSlot == item) {
                return true;
            }
        }
        return false;
    }


    // PROTOTYPE BUTTONS
    public void AddItemRandom() {
        int randomIndex = Random.Range(0,randomItem.Length);
        AddItem(randomItem[randomIndex], 1);
    }
        
    public void RemoveItemRandom() {
            int randomIndex = Random.Range(0,randomItem.Length);
            RemoveItem(randomItem[randomIndex], 1);
    }
}
