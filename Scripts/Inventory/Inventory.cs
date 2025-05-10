using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public Slot[] inventory;
    public Sprite invinsibleSprite;
    public Item[] randomItem;
    public LayerMask terrainLayer;
    public float radius;

    [SerializeField] private Transform itemHolder;

    public int selectedSlot = -1; 
    int? previousSelectedSlot = null;

    public enum InventoryState {
        Success,
        Full
    }

    void UpdateSelectedSlot() {
        for(int i = 0; i < inventory.Length; i++) {
            inventory[i].borderImage.color = new Color32(255,255,255, 255);
        }

        inventory[selectedSlot].borderImage.color = new Color32(255,47,47, 255);

        if(itemHolder.childCount >= 1) {
            Destroy(itemHolder.GetChild(0).gameObject);
        }

        if(inventory[selectedSlot].itemInSlot != null) {
            // Use this on item add
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
            selectedSlot = -1;
            previousSelectedSlot = -2;
        }
    }

    private void SpawnPrefab(Slot slot) {
        if (selectedSlot == -1 || slot != inventory[selectedSlot] || slot.itemInSlot.itemType != Item.ItemType.Tool) return;

        ToolItem itemHolding = (ToolItem)slot.itemInSlot;
        GameObject item = Instantiate(itemHolding.toolPrefab, itemHolder.position, itemHolder.rotation);
        item.transform.SetParent(itemHolder.transform);
        item.GetComponent<Tool>().tool = itemHolding;
    }

    private void RemovePrefab(Slot slot) {
        if (slot == inventory[selectedSlot] && slot.itemInSlot.itemType == Item.ItemType.Tool) Destroy(itemHolder.GetChild(0).gameObject);
    }

    void Update() {
        if (Time.timeScale == 0 || !ObjectLock.active) return;

        if(Input.GetKeyDown(KeyCode.Alpha1)) { selectedSlot = 0; UpdateSelectedSlot(); }
        if(Input.GetKeyDown(KeyCode.Alpha2)) { selectedSlot = 1; UpdateSelectedSlot(); }
        if(Input.GetKeyDown(KeyCode.Alpha3)) { selectedSlot = 2; UpdateSelectedSlot(); }
        if(Input.GetKeyDown(KeyCode.Alpha4)) { selectedSlot = 3; UpdateSelectedSlot(); }
        if(Input.GetKeyDown(KeyCode.Alpha5)) { selectedSlot = 4; UpdateSelectedSlot(); }
        if(Input.GetKeyDown(KeyCode.T) && selectedSlot > -1) inventory[selectedSlot].DropSlot();
    }

    public Dictionary<InventoryState, int> AddItem(Item item, int amount) {
        int tempAmount = amount;
        Dictionary<InventoryState, int> returnState = new Dictionary<InventoryState, int>();

        if (amount <= 0) {
            returnState.Add(InventoryState.Success, 0);
            return returnState;
        }

        if (item.maxStack != 1) {
            Slot[] slots = ContainItems(item);

            slots = slots.Where(slot => slot.amount < item.maxStack).ToArray();

            if (slots.Length != 0) {
                // Add to existing slots
                for (int i = 0; i < slots.Length; i++) {

                    int space = item.maxStack - slots[i].amount;
                    int toAdd = Mathf.Min(space, amount);

                    slots[i].amount += toAdd;
                    slots[i].UpdateUI();
                    tempAmount -= toAdd;

                    if (tempAmount <= 0) {
                        returnState.Add(InventoryState.Success, 0);
                        return returnState;
                    } else if (i == slots.Length - 1) return AddItem(item, tempAmount);
                }
            } else if (!isFull()) {
                // Add new slot
                int remainder = AddNewItem(item, tempAmount);
                return AddItem(item, remainder);
            } else {
                returnState.Add(InventoryState.Full, amount);
                return returnState;
            }
        } else if (!isFull()) {
            // Add new slot
            int remainder = AddNewItem(item, tempAmount);
            return AddItem(item, remainder);
        } else {
            returnState.Add(InventoryState.Full, amount);
            return returnState;
        }

        returnState.Add(InventoryState.Success, 0);
        return returnState;
    }

    private int AddNewItem(Item item, int amount) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i].itemInSlot == null) {
                inventory[i].itemInSlot = item;

                int toAdd = Mathf.Min(item.maxStack, amount);

                inventory[i].amount += toAdd;
                amount -= toAdd;

                inventory[i].UpdateUI();
                SpawnPrefab(inventory[i]);
                return amount;
            }
        }

        return amount;
    }

    public void RemoveItem(Item item, int amount) {
        if(!ContainItem(item)) return;

        for(int i = inventory.Length - 1; i > -1; i--) {
            if(inventory[i].itemInSlot == item) {
                inventory[i].amount -= amount;

                inventory[i].UpdateUI();
                if (inventory[i].amount <= 0) {
                    RemovePrefab(inventory[i]);
                    inventory[i].itemInSlot = null;
                    inventory[i].itemIconImage.sprite = invinsibleSprite;
                }

                return;
            }
        }
    }

    public void RemoveItem(Slot slot, int amount) {
        if(slot.itemInSlot == null) return;

        slot.amount -= amount;

        slot.UpdateUI();
        if (slot.amount <= 0) {
            RemovePrefab(slot);
            slot.itemInSlot = null;
            slot.amount = 0;
            slot.itemIconImage.sprite = invinsibleSprite;
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

    public Slot[] ContainItems(Item item) {
        List<Slot> slots = new List<Slot>();

        for(int i = 0; i < inventory.Length; i++) {
            if(inventory[i].itemInSlot == item) {
                slots.Add(inventory[i]);
            }
        }

        return slots.ToArray();
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
