using System;
using UnityEngine;

// When you add a animation don't forget to add it to the animations array
public enum PlayerAnimations
{
    IDLE,
    RUN,
    DASH,
    DEATH,
    BASIC_ATTACK_01,
    BASIC_ATTACK_02,
    BASIC_ATTACK_03,
    HEAVY_ATTACK,
    BRIEFS_3,
    SLASH_SS,
    CIRCLE_SS,
    HORIZONTAL_SS,
    HAMMER_SPIN_SS,
    THUNDEROUS_HAMMER_SMASH_SS,
    ROCK_STORM_SS,
    WEB_SLAM_SS,
    HAMMER_SMASH_SS,
    RESET
}

public class AnimatorBrain : MonoBehaviour
{
    public readonly int UPPER_BODY_LAYER = 0;
    //public readonly int LOWER_BODY_LAYER = 1;

    public readonly static int[] animations =
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Run"),
        Animator.StringToHash("Dash"),
        Animator.StringToHash("Death"),
        Animator.StringToHash("BasicAttack_01"),
        Animator.StringToHash("BasicAttack_02"),
        Animator.StringToHash("BasicAttack_03"),
        Animator.StringToHash("HeavyAttack"),
        Animator.StringToHash("Briefs_3"),
        Animator.StringToHash("Slash_SS"),
        Animator.StringToHash("Circle_SS"),
        Animator.StringToHash("Horizontal_SS"),
        Animator.StringToHash("Hammer_Spin_SS"),
        Animator.StringToHash("Thunderous_Hammer_Smash_SS"),
        Animator.StringToHash("Rock_Storm_SS"),
        Animator.StringToHash("Web_Slam_SS"),
        Animator.StringToHash("Hammer_Smash_SS"),
    };

    [SerializeField] private PlayerAnimations[] currentAnimation; // It is array because we can have multiple numberOfLayers
    [SerializeField] private bool[] isLayerLocked;

    private Animator animator;
    private PlayerManager playerStateManager;
    private Action onAnimationComplete;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerStateManager = GetComponent<PlayerManager>();

        Initialize(animator.layerCount, PlayerAnimations.IDLE, animator);
    }

    private void Update()
    {
        CheckForAnimationCompletion(UPPER_BODY_LAYER);
    }

    private void CheckForAnimationCompletion(int layerIndex)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        if (stateInfo.normalizedTime >= 1.0f && onAnimationComplete != null)
        {
            onAnimationComplete.Invoke();
            onAnimationComplete = null;
        }
    }


    private void Initialize(int numberOfLayers, PlayerAnimations startingAnimation, Animator animator)
    {
        isLayerLocked = new bool[numberOfLayers];
        currentAnimation = new PlayerAnimations[numberOfLayers];
        this.animator = animator;

        for (int i = 0; i < numberOfLayers; i++)
        {
            isLayerLocked[i] = false;
            currentAnimation[i] = startingAnimation;
        }
    }

    public PlayerAnimations GetCurrentAnimation(int layerIndex)
    {
        return currentAnimation[layerIndex];
    }

    public string GetCurrentAnimationName(int layerIndex)
    {
        return animations[(int)currentAnimation[layerIndex]].ToString();
    }

    public void SetLocked(bool isLayerLocked, int layerIndex)
    {
        this.isLayerLocked[layerIndex] = isLayerLocked;
    }

    public bool IsLocked(int layerIndex)
    {
        return isLayerLocked[layerIndex];
    }

    // Play method to transition to a new animation
    // Parameters:
    // - animation: the animation to play
    // - layerIndex: the layer on which to play the animation
    // - lockLayer: whether to lock the layer after playing the animation
    // - canPassLock: whether this animation can override a locked layer
    // - crossFade: the duration of the crossfade transition
    public void Play(PlayerAnimations animation, int layerIndex, bool lockLayer, bool canPassLock, float crossFade = 0.0f, Action onComplete = null)
    {
        if (animation == PlayerAnimations.RESET)
        {
            PlayDefaultAnimation(); // Reset to the default animation if the animation is RESET
            return;
        }

        if (isLayerLocked[layerIndex] && !canPassLock)
            return; // Do not play the animation if the layer is locked and cannot pass the lock

        isLayerLocked[layerIndex] = lockLayer; // Set the layer lock state

        // Cancel the current animation if this animation can override a locked layer
        if (canPassLock)
        {
            foreach (var behaviour in animator.GetBehaviours<OnAnimationExit>())
            {
                if (behaviour.layerIndex == layerIndex)
                    behaviour.isCancel = true;
            }
        }

        // Do not play the animation if it is already the current animation
        if (currentAnimation[layerIndex] == animation)
            return;

        currentAnimation[layerIndex] = animation; // Set the current animation for the layer
        HandlePlayerState(animation);

        animator.CrossFade(animations[(int)animation], crossFade, layerIndex); // Play the animation with a crossfade

        if (onComplete != null)
        {
            onAnimationComplete = onComplete;
        }
    }

    private void HandlePlayerState(PlayerAnimations animation)
    {
        switch (animation)
        {
            case PlayerAnimations.RUN:
                playerStateManager.SetState(PlayerStateEnum.Running);
                break;
            case PlayerAnimations.DASH:
                playerStateManager.SetState(PlayerStateEnum.Dashing);
                break;
            case PlayerAnimations.DEATH:
                playerStateManager.SetState(PlayerStateEnum.Dead);
                break;
            case PlayerAnimations.BASIC_ATTACK_01:
            case PlayerAnimations.BASIC_ATTACK_02:
            case PlayerAnimations.BASIC_ATTACK_03:
                playerStateManager.SetState(PlayerStateEnum.BasicAttacking);
                break;
            case PlayerAnimations.HEAVY_ATTACK:
            case PlayerAnimations.BRIEFS_3:
                playerStateManager.SetState(PlayerStateEnum.HeavyAttacking);
                break;
            case PlayerAnimations.SLASH_SS:
            case PlayerAnimations.CIRCLE_SS:
            case PlayerAnimations.HORIZONTAL_SS:
            case PlayerAnimations.HAMMER_SPIN_SS:
            case PlayerAnimations.THUNDEROUS_HAMMER_SMASH_SS:
            case PlayerAnimations.ROCK_STORM_SS:
            case PlayerAnimations.WEB_SLAM_SS:
            case PlayerAnimations.HAMMER_SMASH_SS:
                playerStateManager.SetState(PlayerStateEnum.SpeacialAttacking);
                break;
            default:
                playerStateManager.SetState(PlayerStateEnum.Idle);
                break;
        }
    }

    private void PlayDefaultAnimation()
    {
        Play(PlayerAnimations.IDLE, UPPER_BODY_LAYER, false, false);
    }
}
