using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FleeState : BaseState
{
    private float wanderAngle;
    private float lastWanderChange;

    public override void EnterState(StateMachine stateMachine, AIController character)
    {
        wanderAngle = 0;
        lastWanderChange = Time.time;
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

            Vector2 taggedDirection = characterPosition - GameManager.Instance.Tagged.position;
            Vector2 adjustedDirection = (Quaternion.Euler(0, 0, wanderAngle) * taggedDirection).normalized;

            wanderAngle += Random.Range(-stateMachine.WanderRate, stateMachine.WanderRate) * Time.deltaTime;

            if (Time.time > lastWanderChange + stateMachine.ChangeWanderTime)
            {
                wanderAngle *= Random.Range(stateMachine.MaxWanderChange, stateMachine.MinWanderChange);

                lastWanderChange = Time.time;
            }
            
            wanderAngle = Mathf.Clamp(wanderAngle, -stateMachine.MaxWanderAngle, stateMachine.MaxWanderAngle);

            RaycastHit2D forwardHit = Physics2D.Raycast(characterPosition, adjustedDirection, stateMachine.ForwardRaycastLength, stateMachine.WallLayer);

            Vector2 rightDirection = new Vector2(adjustedDirection.y, adjustedDirection.x * -1.0f).normalized;
            RaycastHit2D rightHit = Physics2D.Raycast(characterPosition, rightDirection, stateMachine.SideRaycastLength, stateMachine.WallLayer);

            Vector2 leftDirection = new Vector2(adjustedDirection.y * -1.0f, adjustedDirection.x).normalized;
            RaycastHit2D leftHit = Physics2D.Raycast(characterPosition, leftDirection, stateMachine.SideRaycastLength, stateMachine.WallLayer);

            Vector2 avoidance = Vector2.zero;

            if (forwardHit)
            {
                avoidance = forwardHit.normal * stateMachine.ForwardAvoidanceStrength;
            }

            if (rightHit)
            {
                avoidance += leftDirection * stateMachine.SideAvoidanceStrength / (rightHit.distance == 0 ? stateMachine.SideRaycastLength : rightHit.distance) / 10;
            }
            else if (leftHit)
            {
                avoidance += rightDirection * stateMachine.SideAvoidanceStrength / (leftHit.distance == 0 ? stateMachine.SideRaycastLength : leftHit.distance) / 10;
            }
            else if (forwardHit.distance != 0 && forwardHit.distance < 0.5f)
            {
                avoidance += (Random.Range(-1, 1) < 0 ? leftDirection : rightDirection) * stateMachine.SideAvoidanceStrength * 0.5f;
            }

            adjustedDirection += avoidance;

            character.Move(adjustedDirection);
        }

    }

    public override void ExitState(StateMachine stateMachine, AIController character)
    {

    }
}
