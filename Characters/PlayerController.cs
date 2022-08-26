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

    protected override void OnEnable()
    {
        base.OnEnable();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    void FixedUpdate()
    {
        Move(playerInputActions.Gameplay.Move.ReadValue<Vector2>());
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playerInputActions.Disable();
    }
}
