// --- Simple Component for Resource Nodes ---
// Attach this script to your resource node prefabs (woodNodePrefab, diamondNodePrefab)
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public string resourceType; // e.g., "Wood", "Diamond"
    public int resourceAmount = 1; // Amount of resource this node provides

    /// <summary>
    /// Called when the player interacts with the resource node (e.g., Left Click).
    /// </summary>
    public void Mine()
    {
        Debug.Log("Mined " + resourceAmount + " of " + resourceType + " from " + gameObject.name);

        // TODO: Add logic to:
        // - Add resourceAmount of resourceType to player's inventory
        // - Potentially reduce the resourceAmount on this node
        // - If resourceAmount reaches 0, destroy this GameObject
        Destroy(gameObject); // Destroy the node after mining for simplicity in this example
    }

    // You would likely add logic here to detect the player's mining action,
    // for example, using raycasts from the player's camera on Left Click.
}