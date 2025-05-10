using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour {
    [SerializeField] protected UnityEvent interactionFuction;
    [SerializeField] protected Transform interactionVisual;
    [SerializeField] private float interactDistance;
    [SerializeField] private bool needToLookAt;

    [SerializeField] private float minScale = 0.25f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.1f;
    [SerializeField] private Vector3 originalScale;

    void Awake() {
        originalScale = transform.localScale;
    }

    void Update() {
        if (Time.timeScale == 0) return;

        bool within = Vector3.Distance(Player.player.position, transform.position) <= interactDistance;

        if (interactionVisual) {
            if (ObjectLock.active && within) {
                interactionVisual.gameObject.SetActive(true);
                interactionVisual.LookAt(Player.player.position);
                float scale = minScale + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
                interactionVisual.localScale = originalScale * scale;
            } else interactionVisual.gameObject.SetActive(false);
        }

        if (CameraMove.blocked || !Input.GetKey(KeyCode.F) || !within) return;

        if (needToLookAt) {
            RaycastHit hit;

            Transform camera = Camera.main.transform.parent;

            if (Physics.Raycast(camera.position, transform.TransformDirection(camera.forward), out hit, Mathf.Infinity)) {
                if (hit.transform.gameObject != gameObject) return;
            }
        }

        interactionFuction.Invoke();
    }
}
