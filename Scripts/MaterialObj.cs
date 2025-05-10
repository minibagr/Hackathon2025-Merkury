using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialObj : MonoBehaviour {
    public MaterialItem material;
    public Color particleColor = Color.black;
    public AudioClip sound;
    public AudioClip destroySound;

    [SerializeField] private float health;
    [SerializeField] private float radius;
    [SerializeField] private GameObject dropObject;
    [SerializeField] private Vector2Int dropAmountRange;
    [SerializeField] private LayerMask terrainLayer;

    public void Mine(float damage) {
        health -= damage;

        if (health <= 0) {
            Dictionary<Inventory.InventoryState, int> state = Player.inventory.AddItem(material ,Random.Range(dropAmountRange.x, dropAmountRange.y));

            if (state.ContainsKey(Inventory.InventoryState.Full)) {
                int numOfObjects = state.Values.First();

                for (int i = 0; i < numOfObjects; i++) {
                    Vector3 spawnPosition = new Vector3(
                        transform.position.x + Random.Range(-radius, radius),
                        transform.position.y,
                        transform.position.z + Random.Range(-radius, radius)
                    );

                    RaycastHit hit;
                    if (Physics.Raycast(spawnPosition + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, terrainLayer)) {
                        spawnPosition = hit.point;

                        Vector3 surfaceNormal = hit.normal;

                        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

                        GameObject drop = Instantiate(dropObject, spawnPosition, rotation);

                        drop.transform.position += Vector3.up * drop.transform.lossyScale.y / 2;
                    }
                }
            }

            sound = destroySound;
            Destroy(gameObject);
        }
    }
}
