using UnityEngine;
using System.Collections;
using UnityEngine.AI;

// State responsible for the spider boss's aerial phase where it moves up, spawns minions, and regenerates health
public class SpiderBossUpState : SpiderBossBaseState
{
    private SpiderBossHealth spiderBossHealth;
    private Collider spiderBossCollider;
    private NavMeshAgent navMeshAgent;

    public SpiderBossUpState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData)
    {
        spiderBossHealth = spiderBossStateController.GetComponent<SpiderBossHealth>();
        spiderBossCollider = spiderBossStateController.bossCollider;
        navMeshAgent = spiderBossStateController.navMeshAgent;
    }

    public override void Enter()
    {
        Debug.Log("Entering SpiderUp State");

        if (spiderBossCollider != null)
            spiderBossCollider.enabled = false;

        if (navMeshAgent != null)
            navMeshAgent.enabled = false;


        spiderBossStateController.PlayAnimation("WebClimb");
    }

    public void ClimbEvent()
    {
        CallSpiders();
        spiderBossStateController.StartCoroutine(RegenerateHealthCoroutine());
    }

    private void CallSpiders()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 spawnPosition = spiderBossStateController.transform.position + Random.insideUnitSphere * 5f;
            spawnPosition.y = spiderBossStateController.transform.position.y + 5f;

            GameObject.Instantiate(spiderBossStateController.spiderPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private IEnumerator RegenerateHealthCoroutine()
    {
        // Regenerate boss health 5 times, once per second
        for (int i = 0; i < 5; i++)
        {
            spiderBossHealth.RegenerateHealth(25f);
            yield return new WaitForSeconds(1f);
        }

        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: "WebClimbFall", onAnimationComplete: () =>
        {
            spiderBossStateController.TransitionToState(new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData));
        });
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting SpiderUp State");

        if (spiderBossCollider != null)
            spiderBossCollider.enabled = true;

        if (navMeshAgent != null)
            navMeshAgent.enabled = true;
    }
}
