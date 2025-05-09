using UnityEngine;

public class MaterialObj : MonoBehaviour {
    public MaterialItem material;
    public Color particleColor = Color.black;

    [SerializeField] private float health;
    [SerializeField] private Vector2Int dropAmountRange;

    public void Mine(float damage) {
        health -= damage;

        if (health <= 0) {
            // TODO 1: Check if player has space in inventory else drop on ground
            Player.inventory.AddItem(material ,Random.Range(dropAmountRange.x, dropAmountRange.y));
            Destroy(gameObject);
        }
    }
}
