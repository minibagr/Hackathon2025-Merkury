using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectLock : MonoBehaviour {
    [SerializeField] private GameObject lockOn;
    public static bool active = true;

    public void Update() {
        if (!active) return;

        transform.position = lockOn.transform.position;
    }
}
