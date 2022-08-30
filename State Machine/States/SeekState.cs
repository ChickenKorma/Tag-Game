using UnityEngine;

public class SeekState : BaseState
{
    private Transform previousNearestCharacter;

    private Vector3 previousTargetPosition;

    public override void EnterState(StateMachine stateMachine, BaseController character)
    {
        previousNearestCharacter = null;
    }

    public override void UpdateState(StateMachine stateMachine, BaseController character)
    {
        // State switch condition
        if (!character.Tagged)
        {
            stateMachine.SwitchState(stateMachine.FleeState);
        }


        // Find the nearest character and sets as target
        Transform newNearestCharacter = NearestCharacter(character.transform);

        Vector3 target = newNearestCharacter.position;


        // Attempts to predict the targets movement from the previous frame
        if(previousNearestCharacter == newNearestCharacter && newNearestCharacter != null)
        {
            Vector3 predictedDirection = newNearestCharacter.position - previousTargetPosition;

            float predictionTime = stateMachine.MaxPredictionTime * Mathf.Clamp(predictedDirection.magnitude / stateMachine.MaxPredictionDistance, 0, 1);

            target = newNearestCharacter.position + (predictedDirection.normalized * predictionTime);
        }


        // Moves character towards the target
        Vector3 moveDirection = target - character.transform.position;
        character.Move(moveDirection);


        // Sets variables for next time
        previousNearestCharacter = newNearestCharacter;
        previousTargetPosition = newNearestCharacter.position;
    }

    // Returns the nearest other character to this character
    private Transform NearestCharacter(Transform thisCharacter)
    {
        Transform nearestCharacter = null;

        float nearestSqrDistance = Mathf.Infinity;

        foreach(Transform otherCharacter in GameManager.Instance.RemainingCharacters)
        {
            if (!otherCharacter.Equals(thisCharacter))
            {
                float sqrDistance = Vector3.SqrMagnitude(thisCharacter.position - otherCharacter.position);

                if (sqrDistance < nearestSqrDistance)
                {
                    nearestCharacter = otherCharacter;

                    nearestSqrDistance = sqrDistance;
                }
            }
        }

        return nearestCharacter;
    }
}
