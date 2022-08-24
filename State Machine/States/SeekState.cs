using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SeekState : BaseState
{
    public override void EnterState(StateMachine stateMachine, AIController character)
    {

    }

    public override void UpdateState(StateMachine stateMachine, AIController character)
    {
        if (!character.Tagged)
        {
            stateMachine.SwitchState(stateMachine.FleeState);
        }

        Vector2 direction = NearestCharacter(character.transform).position - character.transform.position;
        character.Move(direction);
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
