using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    public static PlayerController Instance;

    private PlayerInputActions playerInputActions;

    protected override void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        base.Awake();
    }

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    void FixedUpdate()
    {
        Move(playerInputActions.Gameplay.Move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
}
