using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour {
    [SerializeField] protected UnityEvent interactionFuction;
    [SerializeField] private float interactDistance;

    void Update() {
        if (CameraMove.blocked || !Input.GetKeyDown(KeyCode.F) || Vector3.Distance(Player.player.position, transform.position) > interactDistance) return;

        interactionFuction.Invoke();
    }
}
