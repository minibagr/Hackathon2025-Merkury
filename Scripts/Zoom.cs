using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Zoom : MonoBehaviour {
    [SerializeField] private Animation m_Animation;
    [SerializeField] private BaseRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;
    private bool zoom;

    private void Awake() {
        ObjectLock.active = false;
    }

    void Update() {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) {
            if (!zoom || m_Animation.isPlaying) return;

            ObjectLock.active = true;
            return;
        }

        if (ObjectLock.active) return;

        List<RaycastResult> results = new List<RaycastResult>();

        PointerEventData m_PointerEventData = new PointerEventData(eventSystem);
        m_PointerEventData.position = Input.mousePosition;

        raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results) {
            if (result.gameObject.name == "Button") {
                Cursor.lockState = CursorLockMode.Locked;
                result.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }

        if (!zoom || m_Animation.isPlaying) return;

        ObjectLock.active = true;
    }

    public void ActivateZoom() {
        m_Animation.Play();
        zoom = true;
    }
}
