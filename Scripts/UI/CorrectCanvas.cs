using UnityEngine;

public class CorrectCanvas : MonoBehaviour {
    private void Awake() {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
