using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FleeState : BaseState
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
        else if (Vector2.SqrMagnitude(character.transform.position - GameManager.Instance.tagged.position) > character.NearestPickupSqrDistance)
        {
            stateMachine.SwitchState(stateMachine.PickupState);
        }
        */

        if (GameManager.Instance.Tagged)
        {
            Vector2 direction = character.transform.position - GameManager.Instance.Tagged.position;

            RaycastHit2D hit = Physics2D.Raycast(character.transform.position, direction, stateMachine.RaycastLength);

            Vector2 avoidance = Vector2.zero;

            if (hit)
            {
                avoidance = hit.normal * stateMachine.AvoidanceStrength;
            }

            direction += avoidance;

            character.Move(direction);
        }

    }

    public override void ExitState(StateMachine stateMachine, AIController character)
    {

    }
}
