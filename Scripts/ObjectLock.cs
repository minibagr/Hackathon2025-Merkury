using UnityEngine;

public class ObjectLock : MonoBehaviour {
    [SerializeField] private GameObject lockOn;
    [SerializeField] private bool startLockState = true;
    public static bool active = true;

    private void Awake() {
        active = startLockState;
    }

    public void Update() {
        if (!active) return;

        transform.position = lockOn.transform.position;
    }
}
