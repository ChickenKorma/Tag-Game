using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseController characterController;

    [Header("States")]
    private BaseState currentState;

    private SeekState seekState = new();
    private FleeState fleeState = new();

    public SeekState SeekState { get { return seekState; } }
    public FleeState FleeState { get { return fleeState; } }


    [Header("Raycasts")]
    [SerializeField] private float forwardRaycastLength;
    [SerializeField] private float sideRaycastLength;
    [SerializeField] private float forwardAvoidanceStrength;
    [SerializeField] private float sideAvoidanceStrength;   

    public float ForwardRaycastLength { get { return forwardRaycastLength; } }
    public float SideRaycastLength { get { return sideRaycastLength; } }
    public float ForwardAvoidanceStrength { get { return forwardAvoidanceStrength; } }
    public float SideAvoidanceStrength { get { return sideAvoidanceStrength; } }

    [SerializeField] private LayerMask wallLayer;

    public LayerMask WallLayer { get { return wallLayer; } }


    [Header("Predictions")]
    [SerializeField] private float maxPredictionTime;
    [SerializeField] private float maxPredictionDistance;

    public float MaxPredictionTime { get { return maxPredictionTime; } }
    public float MaxPredictionDistance { get { return maxPredictionDistance; } }


    [Header("Wandering")]
    [SerializeField] private float wanderRate;
    [SerializeField] private float maxWanderAngle;
    [SerializeField] private float wanderChangeInterval;
    [SerializeField] private float minWanderChange;
    [SerializeField] private float maxWanderChange;

    public float WanderRate { get { return wanderRate; } }
    public float MaxWanderAngle { get { return maxWanderAngle; } }
    public float WanderChangeInterval { get { return wanderChangeInterval; } }
    public float MinWanderChange { get { return minWanderChange; } }
    public float MaxWanderChange { get { return maxWanderChange; } }


    [Header("Centering")]
    [SerializeField] private float maxCenteringBias;
    [SerializeField] private float maxCenteringDistance;
    [SerializeField] private float minCenteringDistance;
    [SerializeField] private float centeringRandomness;
   
    public float MaxCenteringBias { get { return maxCenteringBias; } }
    public float MaxCenteringDistance { get { return maxCenteringDistance; } }
    public float MinCenteringDistance { get { return minCenteringDistance; } }
    public float CenteringRandomness { get { return centeringRandomness; } }


    private void Awake()
    {
        characterController = GetComponent<BaseController>();
    }

    private void Start()
    {
        currentState = fleeState;
        currentState.EnterState(this, characterController);
    }

    private void FixedUpdate()
    {
        currentState.UpdateState(this, characterController);
    }

    // Switches to the new state
    public void SwitchState(BaseState newState)
    {
        currentState = newState;

        currentState.EnterState(this, characterController);
    }
}
