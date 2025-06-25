using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpecialSkillHandler : MonoBehaviour
{
    [SerializeField] GameObject slashVFX;
    [SerializeField] GameObject circleVFX;
    [SerializeField] GameObject horizontalVFX;
    [SerializeField] GameObject dashVFX;

    private Animator animator;
    private string currentAnimation = "";

    private void Start()
    {
        animator = GetComponent<Animator>();
        DeactivateVFX();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeAnimation("Slash");
            StartCoroutine(VFXPlayer(slashVFX, 2.1f));
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeAnimation("Circle");
            StartCoroutine(VFXPlayer(circleVFX, 3.2f));
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeAnimation("Horizontal");
            StartCoroutine(VFXPlayer(horizontalVFX, 2.1f));
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeAnimation("Dash");
            StartCoroutine(VFXPlayer(dashVFX, 0.6f));
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            ChangeAnimation("Falling");
        }
    }

    public void ChangeAnimation(string animation, float crossFade = 0.2f)
    {
        if (currentAnimation == animation) return;

        animator.CrossFade(animation, crossFade);
    }

    private void DeactivateVFX()
    {
        if (slashVFX != null) slashVFX.SetActive(false);
        if (circleVFX != null) circleVFX.SetActive(false);
        if (horizontalVFX != null) horizontalVFX.SetActive(false);
        if (dashVFX != null) dashVFX.SetActive(false);
    }

    private IEnumerator VFXPlayer(GameObject vfx, float vfxTime)
    {
        if (vfx != null)
        {
            vfx.SetActive(true);
            yield return new WaitForSeconds(vfxTime);
            if (vfx != null) vfx.SetActive(false);
        }
    }
}
