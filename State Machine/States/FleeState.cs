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
            Vector3 characterPosition = character.transform.position;

            Vector2 direction = (characterPosition - GameManager.Instance.Tagged.position).normalized;
            RaycastHit2D forwardHit = Physics2D.Raycast(characterPosition, direction, stateMachine.ForwardRaycastLength, stateMachine.WallLayer);

            Vector2 rightDirection = new Vector2(direction.y, direction.x * -1.0f).normalized;
            RaycastHit2D rightHit = Physics2D.Raycast(characterPosition, rightDirection, stateMachine.SideRaycastLength, stateMachine.WallLayer);

            Vector2 leftDirection = new Vector2(direction.y * -1.0f, direction.x).normalized;
            RaycastHit2D leftHit = Physics2D.Raycast(characterPosition, leftDirection, stateMachine.SideRaycastLength, stateMachine.WallLayer);

            Vector2 avoidance = Vector2.zero;

            if (forwardHit)
            {
                avoidance = forwardHit.normal * stateMachine.ForwardAvoidanceStrength;
            }

            if (rightHit)
            {
                avoidance += leftDirection * stateMachine.SideAvoidanceStrength;
            }
            else if (leftHit)
            {
                avoidance += rightDirection * stateMachine.SideAvoidanceStrength;
            }

            direction += avoidance;

            character.Move(direction);
        }

    }

    public override void ExitState(StateMachine stateMachine, AIController character)
    {

    }
}
