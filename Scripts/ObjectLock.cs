using UnityEngine;

public class ObjectLock : MonoBehaviour {
    [SerializeField] private GameObject lockOn;
    public static bool active = true;

    private void Awake() {
        active = false;
    }

    public void Update() {
        if (!active) return;

        transform.position = lockOn.transform.position;
    }
}
