using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {
    [SerializeField] private Item item;
    [SerializeField] private bool onTrigger;
    [SerializeField] private GameObject sound;

    void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" || !onTrigger) return;

        PickUp();
    }

    public void PickUp() {
        Dictionary<Inventory.InventoryState, int> state = Player.inventory.AddItem(item, 1);

        if (state.ContainsKey(Inventory.InventoryState.Success)) {
            if (sound) Instantiate(sound, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
