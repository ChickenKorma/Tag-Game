using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(StateMachine stateMachine, AIController character);

    public abstract void UpdateState(StateMachine stateMachine, AIController character);

    public abstract void ExitState(StateMachine stateMachine, AIController character);
}
