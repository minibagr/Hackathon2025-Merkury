using UnityEngine;

public class Player : MonoBehaviour
{
    /* --- | Variables | --- */
    public static Transform player;
    public static Inventory inventory;
    public static Quaternion cameraRotation;
    public static float mouseSensitivity = 5f;
    public static bool disableInvetoryUI = true;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private Sound sound;
    [SerializeField] private float soundThreshold;
    [SerializeField] private float clampAngle = 80.0f, rotY, rotX;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float speed = 5f;
    [SerializeField] private CursorLockMode startLockState = CursorLockMode.Locked;
    [SerializeField] private GameObject itemHolder;
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject DisableUI;
    [SerializeField] private Canvas InvetoryUI;

    /* --- | Code | --- */
    public void Start() {
        if (player == null) player = transform;
        if (inventory == null) inventory = transform.parent.GetComponent<Inventory>();
        Cursor.lockState = startLockState;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        cameraRotation = transform.localRotation;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && ObjectLock.active) PauseGame();

        if (Time.timeScale == 0 || !ObjectLock.active) {
            if (moveDir.sqrMagnitude > 0) rb.linearVelocity = Vector3.zero;
            itemHolder.SetActive(false);
            if (disableInvetoryUI) InvetoryUI.enabled = false;
            return;
        } else {
            itemHolder.SetActive(true);
            if (disableInvetoryUI) InvetoryUI.enabled = true;
        }

        /* --- | Camera | --- */
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity;
        rotX += mouseY * mouseSensitivity;

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

        if (moveDir.sqrMagnitude > soundThreshold) sound.PlaySound();
    }

    public void PauseGame() {
        if (Time.timeScale != 0) {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            MenuUI.SetActive(true);
            DisableUI.SetActive(false);
        } else {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            MenuUI.SetActive(false);
            DisableUI.SetActive(true);
        }
    }
}