using UnityEngine;

public class PlayerController : BaseController
{
    public static PlayerController Instance;

    private Vector2 moveInput;

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

        InputManager.moveEvent += OnMoveInput;
    }

    void FixedUpdate()
    {
        Move(moveInput);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        InputManager.moveEvent -= OnMoveInput;
    }

    private void OnDestroy()
    {
        InputManager.moveEvent -= OnMoveInput;
    }

    private void OnMoveInput(Vector2 input) => moveInput = input;
}
