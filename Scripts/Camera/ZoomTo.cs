using UnityEngine;

public class ZoomTo : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private bool block;

    void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            Zoom();
        }
    }

    public void Zoom() {
        if (CameraMove.isMoving) return;

        CameraMove.targetPoint = transform;
        CameraMove.duration = speed;
        CameraMove.targetRot = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        CameraMove.blockIfZoomed = block;
        CameraMove.updated = true;
    }
}
