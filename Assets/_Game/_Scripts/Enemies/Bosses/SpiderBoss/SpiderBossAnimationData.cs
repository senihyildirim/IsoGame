using UnityEngine;

[CreateAssetMenu(fileName = "SpiderBossAnimationData", menuName = "Boss/SpiderBossAnimationData")]
public class SpiderBossAnimationData : BossAnimationData
{
    [Header("Base")]
    public string Encounter = "Encounter";
    public string Walk = "Walk";
    public string Death = "Death";
    public string TakeDamage = "TakeDamage";
    public string TurnLeft90 = "TurnLeft90";
    public string TurnRight90 = "TurnRight90";

    [Header("CloseAttack")]
    public string IdleWithAttack = "IdleWithAttack";
    public string BasicAttack = "BasicAttack";

    [Header("MidAttack")]
    public string SandHoseAttack = "SandHoseAttack";
    public string SandTrap = "SandTrap";

    [Header("RangedAttack")]
    public string RockAttack = "RockAttack";
    public string WebAttack = "WebAttack";
    public string MultipleWebAttack = "MultipleWebAttack";

    [Header("LongRangeAttack")]
    public string DiveIn = "DiveIn";
    public string DiveOut = "DiveOut";

    [Header("Specials")]
    public string WebClimb = "WebClimb";
    public string WebClimbFall = "WebClimbFall";
    public string SandShield = "SandShield";
}

