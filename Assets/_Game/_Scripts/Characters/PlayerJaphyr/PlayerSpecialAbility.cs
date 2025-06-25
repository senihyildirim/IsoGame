using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpecialAbility : MonoBehaviour
{
    private AnimatorBrain animatorBrain;
    private List<PlayerAnimations> specialSkillList;

    private void Start()
    {
        animatorBrain = GetComponent<AnimatorBrain>();

        specialSkillList = new List<PlayerAnimations>
        {
            PlayerAnimations.SLASH_SS,
            PlayerAnimations.CIRCLE_SS,
            PlayerAnimations.HORIZONTAL_SS,
            PlayerAnimations.HAMMER_SPIN_SS,
            PlayerAnimations.THUNDEROUS_HAMMER_SMASH_SS,
            PlayerAnimations.ROCK_STORM_SS,
            PlayerAnimations.WEB_SLAM_SS,
            PlayerAnimations.HAMMER_SMASH_SS,
        };
    }

    // This method is called by the PlayerInput in editor
    public void OnSpecialAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !animatorBrain.IsLocked(animatorBrain.UPPER_BODY_LAYER))
        {
            SpecialAbility();
        }
    }
    //private void SpecialAbility()
    //{
    //    Animations specialSkillAnimationToPlay = GetRandomSpecialSkill();
    //    animatorBrain.Play(specialSkillAnimationToPlay, animatorBrain.UPPER_BODY_LAYER, true, false);
    //}

    //private Animations GetRandomSpecialSkill()
    //{
    //    return specialSkillList[Random.Range(0, specialSkillList.Count)];
    //}
    private void SpecialAbility()
    {
        PlayerAnimations specialSkillAnimationToPlay = GetSpecialSkill();

        animatorBrain.Play(specialSkillAnimationToPlay, animatorBrain.UPPER_BODY_LAYER, true, false, 0.0f);
    }

    private PlayerAnimations GetSpecialSkill()
    {
        string equippedCardName = PlayerAnimations.SLASH_SS.ToString();

        foreach (PlayerAnimations skill in specialSkillList)
        {
            if (skill.ToString().Equals(equippedCardName, StringComparison.OrdinalIgnoreCase))
            {
                return skill;
            }
        }

        return PlayerAnimations.SLASH_SS;
    }
}
