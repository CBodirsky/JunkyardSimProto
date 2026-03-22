using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 5f;

    [Header("Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraPivot;

    private Rigidbody rb;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleLook();
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z) * moveSpeed;
        Vector3 velocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        rb.linearVelocity = velocity;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Simple grounded check
            if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate body horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

}
