using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeavyAttack : MonoBehaviour
{
    private AnimatorBrain animatorBrain;
    private PlayerBasicAttack playerBasicAttack;

    private List<PlayerAnimations> heavySkillList;
    private void Start()
    {
        animatorBrain = GetComponent<AnimatorBrain>();
        playerBasicAttack = GetComponent<PlayerBasicAttack>();

        heavySkillList = new List<PlayerAnimations>
        {
            PlayerAnimations.HEAVY_ATTACK,
            PlayerAnimations.BRIEFS_3
        };
    }

    // This method is called by the PlayerInput in editor
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !animatorBrain.IsLocked(animatorBrain.UPPER_BODY_LAYER))
        {
            HeavyAttack();
        }
    }

    private void HeavyAttack()
    {
        //Debug.Log("Heavy Attack!");
        playerBasicAttack.RotateTowardsMouse();
        PlayerAnimations heavySkillAnimationToPlay = GetSpecialSkill();
        animatorBrain.Play(heavySkillAnimationToPlay, animatorBrain.UPPER_BODY_LAYER, true, false);
    }
    private PlayerAnimations GetSpecialSkill()
    {
        foreach (PlayerAnimations skill in heavySkillList)
        {
            if (skill.ToString().Equals(PlayerAnimations.HEAVY_ATTACK.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return skill;
            }
        }

        return PlayerAnimations.HEAVY_ATTACK;
    }
}
