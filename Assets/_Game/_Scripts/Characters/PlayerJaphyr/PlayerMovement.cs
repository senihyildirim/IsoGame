using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatVariable japhyrMovementSpeed;

    [Header("Debug")]
    // TODO: Change camera handling
    [SerializeField] private Transform cameraTransform; // Reference to the virtual camera's transform

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private float speed;
    private Vector2 moveInput;
    private CharacterController characterController;
    private PlayerManager playerManager;
    private AnimatorBrain animatorBrain;

    private void OnEnable()
    {
        PlayerEvents.OnSpeedBoostRequested += ApplySpeedBoost;
    }

    private void OnDisable()
    {
        PlayerEvents.OnSpeedBoostRequested -= ApplySpeedBoost;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerManager = GetComponent<PlayerManager>();
        animatorBrain = GetComponent<AnimatorBrain>();
        speed = japhyrMovementSpeed.CurrentValue;
    }

    private void Update()
    {
        SnapToGround();
        CheckMovementAnimation();

        if (playerManager.CurrentState == PlayerStateEnum.Running)
        {
            Move();
            UpdateOrientation();
        }

        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    animatorBrain.Play(Animations.DEATH, animatorBrain.UPPER_BODY_LAYER, true, true);
        //}
    }

    private void CheckMovementAnimation()
    {
        if (moveInput.magnitude > 0)
            animatorBrain.Play(PlayerAnimations.RUN, animatorBrain.UPPER_BODY_LAYER, false, false);
        else
            animatorBrain.Play(PlayerAnimations.IDLE, animatorBrain.UPPER_BODY_LAYER, false, false, 0.5f);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;
        moveDirection.Normalize();

        characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    public void SnapToGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, groundCheckDistance, groundLayer))
        {
            Vector3 position = transform.position;
            position.y = hitInfo.point.y;
            transform.position = position;
        }
    }

    private void UpdateOrientation()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 lookDirection = new Vector3(moveInput.x, 0, moveInput.y);
            lookDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * lookDirection;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void ApplySpeedBoost(float amount, bool isTemporary, float duration)
    {
        float originalSpeed = japhyrMovementSpeed.DefaultValue;
        japhyrMovementSpeed.CurrentValue = originalSpeed + amount;

        if (isTemporary && duration > 0)
        {
            StartCoroutine(ResetSpeedAfterDuration(originalSpeed, duration));
        }
    }

    private IEnumerator ResetSpeedAfterDuration(float originalSpeed, float duration)
    {
        yield return new WaitForSeconds(duration);
        japhyrMovementSpeed.CurrentValue = originalSpeed;
    }
}
