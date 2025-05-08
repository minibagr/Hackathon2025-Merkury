// PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    public float jumpForce = 5f;

    [Header("Mining")]
    public float miningRange = 3f;
    public GameObject miningTool;
    public float miningAnimDuration = 0.5f;

    private CharacterController controller;
    private Camera playerCamera;
    private bool controlsEnabled = true;
    private bool isJumping = false;
    private bool isMining = false;
    private float yVelocity = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        if (miningTool != null)
            miningTool.SetActive(false);
    }

    void Update()
    {
        if (!controlsEnabled)
            return;

        // Handle movement
        HandleMovement();

        // Handle mining/interaction
        HandleInteraction();
    }

    void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate move direction in local space
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Handle rotation (if moving)
        if (moveDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
        }

        // Apply gravity
        if (controller.isGrounded)
        {
            yVelocity = -0.5f; // Small constant to keep grounded
            isJumping = false;

            // Handle jump
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpForce;
                isJumping = true;
            }
        }
        else
        {
            // Apply gravity
            yVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // Set final movement vector
        Vector3 motion = transform.forward * vertical + transform.right * horizontal;
        motion = Vector3.ClampMagnitude(motion, 1f) * moveSpeed;
        motion.y = yVelocity;

        // Apply movement
        controller.Move(motion * Time.deltaTime);
    }

    void HandleInteraction()
    {
        // Skip if already mining
        if (isMining)
            return;

        // Check for mining input
        if (Input.GetMouseButtonDown(0))
        {
            // Try to mine whatever is in front of us
            StartCoroutine(MineAnimation());

            // Raycast to see what we're hitting
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, miningRange))
            {
                //Check if it's a resource node
                ResourceNode node = hit.collider.GetComponent<ResourceNode>();
                if (node != null)
                {
                    node.Mine();
                }
            }
        }

        // Check for interaction input
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Raycast to see what we're hitting
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, miningRange))
            {
                // Check for interactable objects
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    System.Collections.IEnumerator MineAnimation()
    {
        // Set mining state
        isMining = true;

        // Show mining tool
        if (miningTool != null)
            miningTool.SetActive(true);

        // Wait for animation
        yield return new WaitForSeconds(miningAnimDuration);

        // Hide mining tool
        if (miningTool != null)
            miningTool.SetActive(false);

        // Reset mining state
        isMining = false;
    }

    public void EnableControls(bool enable)
    {
        controlsEnabled = enable;

        // Stop all movement if disabling
        if (!enable && controller != null)
        {
            yVelocity = 0f;
        }
    }
}

// Interface for interactable objects
public interface IInteractable
{
    void Interact();
}