using System;
using UnityEngine;

public static class PlayerEvents
{
    #region State Events
    // Notifies when player state changes (movement, actions, etc.)
    public static event Action<PlayerStateEnum> OnStateChanged;
    public static void RaiseStateChanged(PlayerStateEnum newState) =>
        OnStateChanged?.Invoke(newState);
    #endregion

    #region Player Status Events
    public static event Action EnablePlayerInputs;
    public static void RaiseEnablePlayerInputs() =>
        EnablePlayerInputs?.Invoke();

    public static event Action DisablePlayerInputs;
    public static void RaiseDisablePlayerInputs() =>
        DisablePlayerInputs?.Invoke();

    public static event Action OnPlayerDeath;
    public static void RaisePlayerDeath() =>
        OnPlayerDeath?.Invoke();
    #endregion

    #region Player Utility Events
    public static event Func<Transform> GetPlayerTransform;
    public static Transform RaiseGetPlayerTransform() =>
        GetPlayerTransform?.Invoke() ?? null;

    public static event Func<Vector3> GetPlayerPosition;
    public static Vector3 RaiseGetPlayerPosition() =>
        GetPlayerPosition?.Invoke() ?? Vector3.zero;
    #endregion

    #region Health Modification Requests
    // Events for requesting health modifications
    public static event Action<float> OnHealRequested;
    public static event Action<float, float, float> OnHealthRegenRequested;
    public static event Action<float> OnMaxHealthIncreaseRequested;

    public static void RequestHeal(float amount) =>
        OnHealRequested?.Invoke(amount);

    public static void RequestHealthRegen(float amountPerTick, float interval, float duration) =>
        OnHealthRegenRequested?.Invoke(amountPerTick, interval, duration);

    public static void RequestMaxHealthIncrease(float amount) =>
        OnMaxHealthIncreaseRequested?.Invoke(amount);
    #endregion

    #region Movement Modification Requests
    public static event Action<float, bool, float> OnSpeedBoostRequested;
    public static void RequestSpeedBoost(float amount, bool isTemporary, float duration) =>
        OnSpeedBoostRequested?.Invoke(amount, isTemporary, duration);
    #endregion

    #region Damage Modification Requests
    // Events for requesting damage modifications
    public static event Action<float, bool, float> OnDamageBoostRequested;
    public static void RequestDamageBoost(float amount, bool isTemporary, float duration) =>
        OnDamageBoostRequested?.Invoke(amount, isTemporary, duration);
    #endregion
}

