using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Camera & Look")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 5f;
    [SerializeField] private float mouseSensitivityAim = 2f;

    [Header("Zoom")]
    [SerializeField] private float normalFOV = 70f;
    [SerializeField] private float minZoomFOV = 20f;
    [SerializeField] private float zoomSpeed = 5f;

    private Vector2 moveInput;
    private Vector2 look;
    private bool onGround;

    private Rigidbody rb;

    private float targetFOV;
    private float currentSensitivity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            playerCamera = FindObjectOfType<Camera>();
            
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Start()
    {
        if (playerCamera != null)
        {
            targetFOV = playerCamera.fieldOfView = normalFOV;
        }
        currentSensitivity = mouseSensitivity;
    }
    private void Update()
    {
        if (playerCamera == null) return;

        ReadInputs();
        HandleJump();
        HandleZoomAndSensitivity();
        LookRotation();
        SmoothFOV();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void ReadInputs()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }
    }
    private void Move()
    {
        if (playerCamera == null) return;

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        Vector3 direction = (right * moveInput.x + forward * moveInput.y).normalized;
        direction.y = 0f;

        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
    }
    private void LookRotation()
    {
        look.x += Input.GetAxis("Mouse X") * currentSensitivity;
        look.y -= Input.GetAxis("Mouse Y") * currentSensitivity;
        look.y = Mathf.Clamp(look.y, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(look.y, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, look.x, 0f);
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
            onGround = true;
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
            onGround = false;
    }
    private void HandleZoomAndSensitivity()
    {
        bool isAiming = Input.GetKey(KeyCode.C);
        targetFOV = isAiming ? minZoomFOV : normalFOV;
        currentSensitivity = isAiming ? mouseSensitivityAim : mouseSensitivity;
    }
    private void SmoothFOV()
    {
        if (playerCamera == null) return;
        
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}