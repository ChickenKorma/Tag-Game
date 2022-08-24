using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : BasePickup
{
    [SerializeField] private float boostFactor;

    private float originalSpeed;

    // Applies effect to the character
    protected override void StartEffect(BaseController character)
    {
        originalSpeed = character.Speed;

        character.Speed = originalSpeed * boostFactor;
    }

    // Reverts character back to original state
    protected override void EndEffect(BaseController character)
    {
        if(character.Speed > originalSpeed)
        {
            character.Speed = originalSpeed;
        }
    }
}
