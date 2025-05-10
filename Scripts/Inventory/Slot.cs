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
        if (amount > 1) amountText.text = amount.ToString();
        else amountText.text = "";
    }

    public void DropSlot() {
        if (!itemInSlot) return;

        for (int i = 0; i < amount; i++) {
            Vector3 spawnPosition = new Vector3(
                Player.player.transform.position.x + Random.Range(-Player.inventory.radius, Player.inventory.radius),
                Player.player.transform.position.y,
                Player.player.transform.position.z + Random.Range(-Player.inventory.radius, Player.inventory.radius)
            );

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, Player.inventory.terrainLayer)) {
                spawnPosition = hit.point;

                Vector3 surfaceNormal = hit.normal;

                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

                GameObject drop = Instantiate(itemInSlot.itemDrop, spawnPosition, rotation);

                drop.transform.position += Vector3.up * drop.transform.lossyScale.y / 2;
            }
        }

        Player.inventory.RemoveItem(this, itemInSlot.maxStack);
    }
}
