using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : BasePickup
{
    // Applies effect to the character
    protected override void StartEffect(BaseController character)
    {
        character.Health = character.MaxHealth;
    }

    // Reverts character back to original state
    protected override void EndEffect(BaseController character)
    {

    }
}
