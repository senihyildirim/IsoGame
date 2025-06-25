using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMobStateMachine
{
    void EnterState(MobBehaviour mob);
    void UpdateState(MobBehaviour mob);
    void ExitState(MobBehaviour mob);
}

