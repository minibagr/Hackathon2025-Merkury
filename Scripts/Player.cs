using UnityEngine;

public class Player : MonoBehaviour
{
    /* --- | Variables | --- */
    public static Transform player;
    public static Quaternion cameraRotation;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 250.0f, clampAngle = 80.0f, rotY, rotX;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float speed = 5f;

    /* --- | Code | --- */
    public void Start() {
        if (player == null) player = transform;
        Cursor.lockState = CursorLockMode.Confined;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        cameraRotation = transform.localRotation;
    }

    private void Update() {
        if (Time.timeScale == 0 || !ObjectLock.active) {
            if (moveDir.sqrMagnitude > 0) rb.linearVelocity = Vector3.zero;
            return;
        }

        /* --- | Camera | --- */
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotY = rotY % 360f;
        rotX = rotX % 360f;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion CameraRotation = Quaternion.Euler(-rotX, rotY, 0);
        Quaternion BodyRotation = Quaternion.Euler(0, rotY, 0);

        playerCamera.rotation = CameraRotation;
        cameraRotation = CameraRotation;
        transform.rotation = BodyRotation;

        /* --- | Movement | --- */
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDir = transform.forward * z + transform.right * x;

        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        Vector3 velocity = moveDir * speed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }
}