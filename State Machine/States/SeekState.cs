using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SeekState : BaseState
{
    private Transform previousNearestCharacter;

    private Vector3 previousPosition;

    public override void EnterState(StateMachine stateMachine, BaseController character)
    {

    }

    public override void UpdateState(StateMachine stateMachine, BaseController character)
    {
        if (!character.Tagged)
        {
            stateMachine.SwitchState(stateMachine.FleeState);
        }

        Transform newNearestCharacter = NearestCharacter(character.transform);

        Vector3 target = newNearestCharacter.position;

        if(previousNearestCharacter == newNearestCharacter)
        {
            Vector3 predictedDirection = (newNearestCharacter.position - previousPosition).normalized;

            float predictionTime = stateMachine.MaxPredictionTime * Mathf.Clamp(Vector3.Distance(newNearestCharacter.position, character.transform.position) / stateMachine.MaxPredictionDistance, 0, 1);

            target = newNearestCharacter.position + (predictedDirection * predictionTime);
        }

        previousNearestCharacter = newNearestCharacter;
        previousPosition = newNearestCharacter.position;

        Vector3 moveDirection = target - character.transform.position;
        character.Move(moveDirection);
    }

    public override void ExitState(StateMachine stateMachine, BaseController character)
    {

    }

    private Transform NearestCharacter(Transform thisCharacter)
    {
        Transform nearest = null;

        float nearestSqrDistance = Mathf.Infinity;

        foreach(Transform otherCharacter in GameManager.Instance.RemainingCharacters)
        {
            if (!otherCharacter.Equals(thisCharacter))
            {
                float sqrDistance = Vector3.SqrMagnitude(thisCharacter.position - otherCharacter.position);

                if (sqrDistance < nearestSqrDistance)
                {
                    nearest = otherCharacter;

                    nearestSqrDistance = sqrDistance;
                }
            }
        }

        return nearest;
    }
}
