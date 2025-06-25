using UnityEngine;
using System.Collections;

public class SpiderBossSandShieldState : SpiderBossBaseState
{
    public SpiderBossSandShieldState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Entering SandShield State");
        spiderBossStateController.PlayAnimation(spiderBossAnimationData.SandShield);
        SpawnShield();
        spiderBossStateController.GetComponent<SpiderBossHealth>().isShieldActive = true;
    }

    private void SpawnShield()
    {
        GameObject activeShield = GameObject.Instantiate(spiderBossStateController.shieldPrefab, spiderBossStateController.transform.position, Quaternion.identity);
        GameObject.Destroy(activeShield, 6f);
        spiderBossStateController.StartCoroutine(ShieldDuration(6f));
    }

    private IEnumerator ShieldDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        spiderBossStateController.GetComponent<SpiderBossHealth>().isShieldActive = false;
        spiderBossStateController.TransitionToState(new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData));
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting SandShield State");
    }
}
