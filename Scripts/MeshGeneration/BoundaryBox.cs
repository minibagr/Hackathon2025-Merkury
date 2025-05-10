using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoundaryBox : MonoBehaviour
{
    public Transform player;
    public float boundarySize = 100f; // Set via script from TerrainGenerator

    void Start()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(boundarySize, 100f, boundarySize);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            player.position = transform.position + dir * (boundarySize * 0.49f); // Push back inside
        }
    }
}