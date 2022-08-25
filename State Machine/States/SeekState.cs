using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SeekState : BaseState
{
    private Transform previousNearestCharacter;

    private Vector3 previousPosition;

    public override void EnterState(StateMachine stateMachine, AIController character)
    {

    }

    public override void UpdateState(StateMachine stateMachine, AIController character)
    {
        if (!character.Tagged)
        {
            stateMachine.SwitchState(stateMachine.FleeState);
        }

        Transform newNearestCharacter = NearestCharacter(character.transform);

        Vector2 target = newNearestCharacter.position;

        if(previousNearestCharacter == newNearestCharacter)
        {
            Vector2 predictedDirection = (newNearestCharacter.position - previousPosition).normalized;

            float predictionTime = stateMachine.MaxPredictionTime * Mathf.Clamp(Vector3.Distance(newNearestCharacter.position, character.transform.position) / stateMachine.MaxPredictionDistance, 0, 1);

            target = newNearestCharacter.position.ConvertTo<Vector2>() + (predictedDirection * predictionTime);
        }

        previousNearestCharacter = newNearestCharacter;
        previousPosition = newNearestCharacter.position;

        Vector2 moveDirection = target - character.transform.position.ConvertTo<Vector2>();
        character.Move(moveDirection);
    }

    public override void ExitState(StateMachine stateMachine, AIController character)
    {

    }

    private Transform NearestCharacter(Transform thisCharacter)
    {
        Transform nearest = null;

        float nearestSqrDistance = Mathf.Infinity;

        foreach(Transform visibleCharacter in GameManager.Instance.VisibleCharacters)
        {
            if (!visibleCharacter.Equals(thisCharacter))
            {
                float sqrDistance = Vector3.SqrMagnitude(thisCharacter.position - visibleCharacter.position);

                if (sqrDistance < nearestSqrDistance)
                {
                    nearest = visibleCharacter;

                    nearestSqrDistance = sqrDistance;
                }
            }
        }

        return nearest;
    }
}
