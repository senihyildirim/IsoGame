using System.Collections;
using UnityEngine;

public class OnAnimationExit : StateMachineBehaviour
{
    [SerializeField] private PlayerAnimations animation;
    [SerializeField] private bool lockLayer;
    [SerializeField] private float crossfade = 0.2f;
    [SerializeField] private bool isComboAttack = false;

    [HideInInspector] public bool isCancel = false;
    [HideInInspector] public int layerIndex = -1;

    // OnStateEnter is called when a transition starts and the state machine starts evaluating this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.layerIndex = layerIndex;
        isCancel = false;

        AnimatorBrain animatorBrain = animator.GetComponentInParent<AnimatorBrain>();
        animatorBrain.StartCoroutine(Wait(stateInfo.length / stateInfo.speedMultiplier - crossfade, animator));
    }

    private IEnumerator Wait(float waitTime, Animator animator)
    {
        // If get input can continue the combo or drop the combo
        AnimatorBrain animatorBrain = animator.GetComponentInParent<AnimatorBrain>();
        PlayerBasicAttack playerBasicAttack = animator.GetComponentInParent<PlayerBasicAttack>();

        yield return new WaitForSeconds(waitTime);

        if (isComboAttack)
        {
            // If input getted continue the combo
            if (playerBasicAttack.isComboInputGetted)
            {
                animatorBrain.SetLocked(false, layerIndex);
                playerBasicAttack.Attack();
                yield break;
            }
        }

        playerBasicAttack.ResetCombo();

        // Check if the state transition was cancelled
        if (isCancel)
            yield break;

        animatorBrain.SetLocked(false, layerIndex);
        animatorBrain.Play(PlayerAnimations.IDLE, layerIndex, lockLayer, false, 0.5f);
    }
}
