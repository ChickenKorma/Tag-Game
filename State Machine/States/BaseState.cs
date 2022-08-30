public abstract class BaseState
{
    public abstract void EnterState(StateMachine stateMachine, BaseController characterController);

    public abstract void UpdateState(StateMachine stateMachine, BaseController characterController);
}
