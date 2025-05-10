using UnityEngine;

public class CameraMove : MonoBehaviour {
    public static Transform targetPoint;
    public static Transform cameraPivot;
    public static bool updated;
    public static float duration;
    public static Quaternion targetRot;
    public static bool isMoving = false;
    public static bool blockIfZoomed;
    public static bool blocked = true;

    private Vector3 startPos;
    private Quaternion startRot;
    private float elapsedTime = 0f;
    private bool lockState;
    [SerializeField] private bool blockedState = false;

    private void Awake() {
        blocked = blockedState;
    }

    public void MoveCamera() {
        lockState = !ObjectLock.active;
        ObjectLock.active = false;

        Cursor.lockState = CursorLockMode.Locked;

        if (!cameraPivot) cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot").transform;

        if (transform.position == targetPoint.position) {
            targetPoint = cameraPivot;
            targetRot = Player.cameraRotation;
            blocked = false;
        } else if (blockIfZoomed) blocked = true;

        startPos = transform.position;
        startRot = transform.rotation;
        elapsedTime = 0f;
        updated = false;
        isMoving = true;
    }

    void Update() {
        if (!isMoving) {
            if (updated) MoveCamera();
            return;
        } else updated = false;

        elapsedTime += Time.deltaTime;
        float time = Mathf.Clamp01(elapsedTime / duration);

        transform.position = Vector3.Lerp(startPos, targetPoint.position, time);
        transform.rotation = Quaternion.Slerp(startRot, targetRot, time);

        if (time >= 1f) {
            isMoving = false;
            ObjectLock.active = lockState;
            if (!ObjectLock.active) Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
