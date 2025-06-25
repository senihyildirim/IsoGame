using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicAttack : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatVariable japhyrBasicAttackDamage;

    [Header("Combo Settings")]
    public int comboStep = 0;
    public bool isComboInputGetted = false;
    public bool isStartWithFirstAttack = true;
    public int maxComboStep { get; private set; } = 2;

    private PlayerManager playerStateManager;
    private AnimatorBrain animatorBrain;
    private Camera mainCamera;

    private void OnEnable()
    {
        PlayerEvents.OnDamageBoostRequested += HandleDamageBoost;
    }

    private void OnDisable()
    {
        PlayerEvents.OnDamageBoostRequested -= HandleDamageBoost;
    }

    private void Start()
    {
        playerStateManager = GetComponent<PlayerManager>();
        animatorBrain = GetComponent<AnimatorBrain>();
        mainCamera = Camera.main;
        ResetCombo();
    }

    // This method is called by the PlayerInput in editor
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && (playerStateManager.CurrentState == PlayerStateEnum.BasicAttacking || !animatorBrain.IsLocked(animatorBrain.UPPER_BODY_LAYER)))
        {
            if (isStartWithFirstAttack)
            {
                Attack();
                return;
            }

            isComboInputGetted = true;
        }
    }

    public void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 direction = hitInfo.point - transform.position;
            direction.y = 0f; // Keep the character rotation on the horizontal plane
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void Attack()
    {
        isComboInputGetted = false;
        isStartWithFirstAttack = false;

        RotateTowardsMouse();
        PlayerAnimations currentAnimation = GetComboAttack();
        animatorBrain.Play(currentAnimation, animatorBrain.UPPER_BODY_LAYER, true, false);

        comboStep = (comboStep + 1) % 3;
    }

    private PlayerAnimations GetComboAttack()
    {
        switch (comboStep)
        {
            case 0:
                return PlayerAnimations.BASIC_ATTACK_01;
            case 1:
                return PlayerAnimations.BASIC_ATTACK_02;
            case 2:
                return PlayerAnimations.BASIC_ATTACK_03;
            default:
                return PlayerAnimations.IDLE;
        }
    }

    public void ResetCombo()
    {
        //Debug.Log("Combo reset");
        comboStep = 0;
        isComboInputGetted = false;
        isStartWithFirstAttack = true;
    }

    private void HandleDamageBoost(float amount, bool isTemporary, float duration)
    {
        float originalDamage = japhyrBasicAttackDamage.DefaultValue;

        // Apply the damage boost
        japhyrBasicAttackDamage.CurrentValue = originalDamage + amount;

        // If the boost is temporary, start a coroutine to reset the damage after the duration
        if (isTemporary && duration > 0)
        {
            StartCoroutine(ResetDamageAfterDuration(duration, originalDamage));
        }
    }

    // Coroutine to reset the damage after the boost duration ends
    private IEnumerator ResetDamageAfterDuration(float duration, float originalDamage)
    {
        yield return new WaitForSeconds(duration);

        // Reset attack damage back to the original value
        japhyrBasicAttackDamage.CurrentValue = originalDamage;
    }
}
