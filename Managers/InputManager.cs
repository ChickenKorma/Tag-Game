using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public static Action<Vector2> moveEvent = delegate { };
    public static Action pauseEvent = delegate { };

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Gameplay.Move.started += OnMove;
        playerInputActions.Gameplay.Move.performed += OnMove;
        playerInputActions.Gameplay.Move.canceled += OnMove;

        playerInputActions.Gameplay.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        playerInputActions.Gameplay.Move.started -= OnMove;
        playerInputActions.Gameplay.Move.performed -= OnMove;
        playerInputActions.Gameplay.Move.canceled -= OnMove;

        playerInputActions.Gameplay.Pause.performed -= OnPause;

        playerInputActions.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveEvent.Invoke(context.ReadValue<Vector2>());
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        pauseEvent.Invoke();
    }
}
