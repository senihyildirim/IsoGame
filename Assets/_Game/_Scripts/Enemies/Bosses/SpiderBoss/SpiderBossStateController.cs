using UnityEngine;

public class SpiderBossStateController : BossStateController
{
    [Header("Animation Data")]
    [SerializeField] private SpiderBossAnimationData spiderBossAnimationData;

    [Header("Projectile Settings")]
    public GameObject webPrefab;
    public GameObject hosePrefab;

    [Header("Spider Call Settings")]
    public GameObject spiderPrefab;

    [Header("Shield Settings")]
    public GameObject shieldPrefab;

    [Header("Sand Trap Settings")]
    public GameObject sandTrapPrefab;

    public bool canEnterUpState = true;
    public bool canEnterShieldState = true;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Spider Boss State Controller Start? " + this.currentStateName);
        stateMachine.Initialize(new SpiderBossDisabledState(this, spiderBossAnimationData));
    }

    public void HandleSpiderBossDeath()
    {
        TransitionToState(new SpiderBossDeathState(this, spiderBossAnimationData));
    }
}
