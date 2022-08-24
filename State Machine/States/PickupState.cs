using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupState : BaseState
{
    public override void EnterState(StateMachine stateMachine, AIController character)
    {

    }

    public override void UpdateState(StateMachine stateMachine, AIController character)
    {
        if (character.Tagged)
        {
            stateMachine.SwitchState(stateMachine.SeekState);
        }
        /*
        else if (Vector2.SqrMagnitude(character.transform.position - GameManager.Instance.Tagged.position) < character.NearestPickupSqrDistance)
        {
            stateMachine.SwitchState(stateMachine.PickupState);
        }
        */
    }

    public override void ExitState(StateMachine stateMachine, AIController character)
    {

    }
}
