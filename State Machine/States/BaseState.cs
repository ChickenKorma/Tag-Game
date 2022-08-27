using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(StateMachine stateMachine, BaseController character);

    public abstract void UpdateState(StateMachine stateMachine, BaseController character);

    public abstract void ExitState(StateMachine stateMachine, BaseController character);
}
