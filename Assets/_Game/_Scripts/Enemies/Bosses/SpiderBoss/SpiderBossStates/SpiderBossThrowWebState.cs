using UnityEngine;
using System.Collections;

public class SpiderBossThrowWebState : SpiderBossBaseState
{
    private const int NUMBER_OF_WEBS = 10;
    private const float WEB_SPEED = 20f;
    private const float ARC_ANGLE = 120f;

    public SpiderBossThrowWebState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Entering Throw Web State");

        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: spiderBossAnimationData.WebAttack, onAnimationComplete: () =>
        {
            spiderBossStateController.TransitionToState(new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData));

        });
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting Throw Web State");
    }

    public void ThrowWebEvent()
    {
        ThrowWebsInCircle();
    }

    private void ThrowWebsInCircle()
    {
        Vector3 shootPosition = spiderBossStateController.transform.position;
        Vector3 directionToPlayer = GetDirectionToPlayer(shootPosition);

        float angleStep = ARC_ANGLE / (NUMBER_OF_WEBS - 1);
        float startAngle = -ARC_ANGLE / 2;

        for (int i = 0; i < NUMBER_OF_WEBS; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            ThrowSingleWeb(shootPosition, directionToPlayer, currentAngle);
        }
    }

    private Vector3 GetDirectionToPlayer(Vector3 shootPosition)
    {
        Vector3 playerPosition = PlayerEvents.RaiseGetPlayerPosition();
        return (playerPosition - shootPosition).normalized;
    }

    private void ThrowSingleWeb(Vector3 position, Vector3 direction, float angle)
    {
        // Assuming webPrefab is set in the SpiderBossStateController
        GameObject web = Object.Instantiate(spiderBossStateController.webPrefab, position, Quaternion.identity);
        Vector3 rotatedDirection = Quaternion.Euler(0, angle, 0) * direction;

        SetupWebPhysics(web, rotatedDirection);
    }

    private void SetupWebPhysics(GameObject web, Vector3 direction)
    {
        Rigidbody webRb = web.GetComponent<Rigidbody>();
        if (webRb == null)
        {
            Debug.LogError("WebPrefab does not have a Rigidbody component!");
            return;
        }

        webRb.isKinematic = false;
        webRb.useGravity = false;
        webRb.velocity = direction * WEB_SPEED;
    }
}