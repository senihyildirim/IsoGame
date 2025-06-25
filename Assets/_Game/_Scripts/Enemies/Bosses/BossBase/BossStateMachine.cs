public class BossStateMachine
{
    public BossBaseState CurrentState { get; private set; }

    public void Initialize(BossBaseState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BossBaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
