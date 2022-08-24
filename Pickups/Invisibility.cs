using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Invisibility : BasePickup
{
    private Color normalColor;
    private Color tagColor;

    // Applies effect to the character
    protected override void StartEffect(BaseController character)
    {
        normalColor = character.NormalColor;
        tagColor = character.TagColor;

        if(character.GetType() == typeof(PlayerController))
        {
            SetAlpha(0.25f);
        }
        else
        {
            SetAlpha(0);
        }

        SetColor(character);      

        GameManager.Instance.HideCharacter(character.transform);
    }

    // Reverts character back to original state
    protected override void EndEffect(BaseController character)
    {
        SetAlpha(1);

        SetColor(character);

        GameManager.Instance.ShowCharacter(character.transform);
    }

    // Sets the character colors equal to the stored colors and updates the sprite renderer color
    private void SetColor(BaseController character)
    {
        character.NormalColor = normalColor;
        character.TagColor = tagColor;

        character.UpdateColor();
    }

    // Sets the alpha channel of the stored colors to the input float
    private void SetAlpha(float alpha)
    {
        normalColor.a = alpha;
        tagColor.a = alpha;
    }
}
