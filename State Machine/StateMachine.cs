using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState currentState;

    private SeekState seekState = new();
    private FleeState fleeState = new();
    private PickupState pickupState = new();

    public SeekState SeekState { get { return seekState; } }
    public FleeState FleeState { get { return fleeState; } }
    public PickupState PickupState { get { return pickupState; } }

    private AIController character;

    [SerializeField] private float raycastLength;
    [SerializeField] private float avoidanceStrength;

    public float RaycastLength { get { return raycastLength; } }
    public float AvoidanceStrength { get { return avoidanceStrength; } }

    [SerializeField] private LayerMask wallLayer;

    public LayerMask WallLayer { get { return wallLayer; } }

    void Start()
    {
        character = GetComponent<AIController>();

        currentState = fleeState;
        currentState.EnterState(this, character);
    }

    void Update()
    {
        currentState.UpdateState(this, character);
    }

    // Sets current state to the new state, calling the exit and enter states accordingly
    public void SwitchState(BaseState newState)
    {
        currentState.ExitState(this, character);

        currentState = newState;

        currentState.EnterState(this, character);
    }
}
