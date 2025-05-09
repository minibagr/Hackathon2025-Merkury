using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour {
    [SerializeField] protected UnityEvent interactionFuction;
    [SerializeField] protected Transform interactionVisual;
    [SerializeField] private float interactDistance;

    [SerializeField] private float minScale = 0.25f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.1f;
    [SerializeField] private Vector3 originalScale;

    void Awake() {
        originalScale = transform.localScale;
    }

    void Update() {
        bool within = Vector3.Distance(Player.player.position, transform.position) <= interactDistance;

        if (interactionVisual) {
            if (ObjectLock.active && within) {
                interactionVisual.gameObject.SetActive(true);
                interactionVisual.LookAt(Player.player.position);
                float scale = minScale + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
                interactionVisual.localScale = originalScale * scale;
            } else interactionVisual.gameObject.SetActive(false);
        }
        

        if (CameraMove.blocked || !Input.GetKeyDown(KeyCode.F) || !within) return;

        interactionFuction.Invoke();
    }
}
