using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Animation Event Channel")]
public class AnimationEventChannel : ScriptableObject
{
    public UnityAction<PlayerAnimations, int> OnAnimationRequested;
    public UnityAction<int> OnAnimationComplete;
    public UnityAction<bool, int> OnLayerLockRequested;

    public void RaiseAnimationRequest(PlayerAnimations anim, int layer) =>
        OnAnimationRequested?.Invoke(anim, layer);

    public void RaiseAnimationComplete(int layer) =>
        OnAnimationComplete?.Invoke(layer);

    public void RaiseLayerLockRequest(bool locked, int layer) =>
        OnLayerLockRequested?.Invoke(locked, layer);
}