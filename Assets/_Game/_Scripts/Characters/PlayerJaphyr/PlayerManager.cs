using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Japhyr Stats")]
    [SerializeField] private FloatVariable japhyrHealth;
    [SerializeField] private FloatVariable japhyrMovementSpeed;
    [SerializeField] private FloatVariable japhyrBasicAttackDamage;

    [Space(10)]
    [SerializeField] private PlayerStateEnum currentState = PlayerStateEnum.Idle;

    public PlayerStateEnum CurrentState
    {
        get => currentState;
        private set
        {
            currentState = value;
            OnStateChange?.Invoke();
        }
    }

    public delegate void StateChangeDelegate();
    public event StateChangeDelegate OnStateChange;

    public void SetState(PlayerStateEnum newState)
    {
        if (currentState != newState)
        {
            CurrentState = newState;
        }
    }

    private void OnEnable()
    {
        PlayerEvents.GetPlayerTransform += GetPlayerTransform;
        PlayerEvents.GetPlayerPosition += GetPlayerPosition;
        PlayerEvents.EnablePlayerInputs += EnableJaphyrInputs;
        PlayerEvents.DisablePlayerInputs += DisableJaphyrInputs;
    }

    private void OnDisable()
    {
        PlayerEvents.GetPlayerTransform -= GetPlayerTransform;
        PlayerEvents.GetPlayerPosition -= GetPlayerPosition;
        PlayerEvents.EnablePlayerInputs -= EnableJaphyrInputs;
        PlayerEvents.DisablePlayerInputs -= DisableJaphyrInputs;

        japhyrHealth.Reset();
        japhyrMovementSpeed.Reset();
        japhyrBasicAttackDamage.Reset();
    }

    private Transform GetPlayerTransform()
    {
        return transform;
    }

    private Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    private void EnableJaphyrInputs()
    {
        GetComponent<PlayerInput>().currentActionMap.Enable();
    }

    private void DisableJaphyrInputs()
    {
        GetComponent<PlayerInput>().currentActionMap.Disable();
    }
}

public enum PlayerStateEnum
{
    Idle,
    Running,
    Dashing,
    Dead,
    BasicAttacking,
    HeavyAttacking,
    SpeacialAttacking,
}
