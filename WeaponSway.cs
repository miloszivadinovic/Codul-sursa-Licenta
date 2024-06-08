using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] public Transform weaponTransform;

    [Header("Sway Properties")]
    [SerializeField] private float swayAmount = 0.01f;
    [SerializeField] public float maxSwayAmount = 0.1f;
    [SerializeField] public float swaySmooth = 9f;
    [SerializeField] public AnimationCurve swayCurve;

    [Range(0f, 1f)]
    [SerializeField] public float swaySmoothCounteraction = 1f;

    [Header("Rotation")]
    [SerializeField] public float rotationSwayMultiplier = 1f;

    [Header("Position")]
    [SerializeField] public float positionSwayMultiplier = -1f;

    [Header("Bobbing Properties")]
    [SerializeField] private float bobbingAmount = 0.1f;
    [SerializeField] private float bobbingSpeed = 0.2f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector2 sway;

    private bool isMoving = false;
    private float bobbingTimer = 0f;

    private void Reset()
    {
        Keyframe[] ks = new Keyframe[] { new Keyframe(0, 0, 0, 2), new Keyframe(1, 1) };
        swayCurve = new AnimationCurve(ks);
    }

    private void Start()
    {
        //checks if the weapon transform is not assigned 
        if (!weaponTransform)
            weaponTransform = transform;
        initialPosition = weaponTransform.localPosition;
        initialRotation = weaponTransform.localRotation; //stores initial pos and rot
    }
    //used lateupdate so the animator doesn't overwrite the update method, preventing the weaponsway from working properly
    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        sway = Vector2.MoveTowards(sway, Vector2.zero, swayCurve.Evaluate(Time.deltaTime * swaySmoothCounteraction * sway.magnitude * swaySmooth));
        sway = Vector2.ClampMagnitude(new Vector2(mouseX, mouseY) + sway, maxSwayAmount);

        weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, new Vector3(sway.x, sway.y, 0) * positionSwayMultiplier + initialPosition, swayCurve.Evaluate(Time.deltaTime * swaySmooth));
        weaponTransform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation * Quaternion.Euler(Mathf.Rad2Deg * rotationSwayMultiplier * new Vector3(-sway.y, sway.x, 0)), swayCurve.Evaluate(Time.deltaTime * swaySmooth));

        HandleBobbing();
    }

    private void HandleBobbing()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            isMoving = true;
            bobbingTimer += bobbingSpeed * Time.deltaTime;
        }
        else
        {
            isMoving = false;
            bobbingTimer = 0f;

            // If the player stops moving, reset the weapon position instantly
            weaponTransform.localPosition = new Vector3(weaponTransform.localPosition.x, initialPosition.y, weaponTransform.localPosition.z);
            return;
        }

        float waveSlice = 0.0f;
        float horizontal = horizontalInput;
        float vertical = verticalInput;

        if (isMoving)
        {
            waveSlice = Mathf.Sin(bobbingTimer);
            bobbingTimer += bobbingSpeed * Time.deltaTime;
        }
        else
        {
            if (bobbingTimer > Mathf.PI * 2)
            {
                bobbingTimer = 0f;
            }
        }

        if (waveSlice != 0)
        {
            float translateChange = waveSlice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            // Apply bobbing to the weapon's position
            weaponTransform.localPosition = new Vector3(weaponTransform.localPosition.x,
                                                         initialPosition.y + translateChange,
                                                         weaponTransform.localPosition.z);
        }
        else
        {
            weaponTransform.localPosition = new Vector3(weaponTransform.localPosition.x,
                                                         initialPosition.y,
                                                         weaponTransform.localPosition.z);
        }
    }


}
