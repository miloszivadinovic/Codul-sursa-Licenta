using UnityEngine;
using TMPro;


public class SimplePlayerMovement : MonoBehaviour
{
    // Assignables
    public Transform playerCam;
    public Transform orientation;

    // Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    // Movement
    public float moveSpeed = 10f;
    public float sprintSpeed = 15f;
    public float airMoveSpeed = 5f; // Speed when moving in the air
    public float staminaMax = 100f;
    public float staminaRegenRate = 10f;
    public float sprintStaminaCost = 20f;
    private float currentStamina;
    private bool isSprinting = false;

    // Jumping
    public float jumpForce = 10f;
    public float jumpStaminaCost = 10f;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private float groundCheckDistance = 0.4f;
    private Transform groundCheck;

    // Slow Motion
    public float slowMotionFactor = 0.2f;
    public float slowMotionStaminaCost = 5f;
    private bool isSlowMotionActive = false;

    // UI
    public TMP_Text staminaText;

    // Input
    float x, y;

    private Rigidbody rb;
    private PhysicMaterial zeroFrictionMaterial;
    private PhysicMaterial highFrictionMaterial;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina = staminaMax;

        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        groundCheck = new GameObject("GroundCheck").transform;
        groundCheck.SetParent(transform);
        groundCheck.localPosition = new Vector3(0, -1, 0);

        // Create and configure physic materials
        zeroFrictionMaterial = new PhysicMaterial();
        zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        zeroFrictionMaterial.dynamicFriction = 0f;
        zeroFrictionMaterial.staticFriction = 0f;

        highFrictionMaterial = new PhysicMaterial();
        highFrictionMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
        highFrictionMaterial.dynamicFriction = 1f;
        highFrictionMaterial.staticFriction = 1f;
    }

    private void Update()
    {
        MyInput();
        Look();

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, whatIsGround);

        // Toggle sprinting when left shift is pressed and there's enough stamina
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSprinting = !isSprinting;
        }

        // Sprint if sprint key is held and there's enough stamina
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSprinting = true;
            currentStamina -= (staminaRegenRate + sprintStaminaCost) * Time.deltaTime;
        }
        else
        {
            isSprinting = false;
        }

        if (currentStamina <= 0)
        {
            isSprinting = false;
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentStamina >= jumpStaminaCost)
        {
            Jump();
        }

        // Handle slow motion
        if (Input.GetKeyDown(KeyCode.V) && currentStamina > 0)
        {
            ToggleSlowMotion();
        }

        if (isSlowMotionActive)
        {
            currentStamina -= slowMotionStaminaCost * Time.unscaledDeltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                ToggleSlowMotion();
            }
        }

        // Regenerate stamina
        if (currentStamina < staminaMax && !isSlowMotionActive)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, staminaMax);
        }

        // Update UI
        UpdateStaminaUI();

        // Apply friction control based on grounded state
        Collider playerCollider = GetComponent<Collider>();
        playerCollider.material = isGrounded ? highFrictionMaterial : zeroFrictionMaterial;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier / (isSlowMotionActive ? slowMotionFactor : 1f);
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier / (isSlowMotionActive ? slowMotionFactor : 1f);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, playerCam.transform.localRotation.eulerAngles.y + mouseX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, orientation.transform.localRotation.eulerAngles.y + mouseX, 0);
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = orientation.transform.forward * y + orientation.transform.right * x;
        float speed = isGrounded ? (isSprinting ? sprintSpeed : moveSpeed) : airMoveSpeed;

        Vector3 targetVelocity = moveDirection.normalized * speed;
        Vector3 velocityChange = targetVelocity - rb.velocity;
        velocityChange.y = 0;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            currentStamina -= jumpStaminaCost;
        }
    }

    private void ToggleSlowMotion()
    {
        isSlowMotionActive = !isSlowMotionActive;
        Time.timeScale = isSlowMotionActive ? slowMotionFactor : 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void UpdateStaminaUI()
    {
        if (staminaText != null)
        {
            staminaText.text = Mathf.FloorToInt(currentStamina) + " / " + staminaMax;
        }
    }
}