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

    [SerializeField] private float forwardRaycastLength;
    [SerializeField] private float sideRaycastLength;
    [SerializeField] private float forwardAvoidanceStrength;
    [SerializeField] private float sideAvoidanceStrength;   

    public float ForwardRaycastLength { get { return forwardRaycastLength; } }
    public float SideRaycastLength { get { return sideRaycastLength; } }
    public float ForwardAvoidanceStrength { get { return forwardAvoidanceStrength; } }
    public float SideAvoidanceStrength { get { return sideAvoidanceStrength; } }   

    [SerializeField] private float maxPredictionTime;
    [SerializeField] private float maxPredictionDistance;

    public float MaxPredictionTime { get { return maxPredictionTime; } }
    public float MaxPredictionDistance { get { return maxPredictionDistance; } }

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
