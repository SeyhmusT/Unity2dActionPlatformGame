using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbAbility : BaseAbility
{

    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float setMinClimbTime;
    private float minimumClimbTime;
    private bool climb;
    public bool canGoOnLadder;
    private string climbAnimParameterName = "Climb";
    private int climbParameterInt;

    protected override void Initialization()
    {
        base.Initialization();
        minimumClimbTime = setMinClimbTime;
        climbParameterInt = Animator.StringToHash(climbAnimParameterName);
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        climb = false;
        linkedAnimator.enabled = true;
        player.ActivateCurrentWeapon();

    }
    public override void EnterAbility()
    {
        player.DeactivateCurrentWeapon();
    }
    private void OnEnable()
    {
        ladderActionRef.action.performed += TryToClimb;
        ladderActionRef.action.canceled += StopClimb;
    }

    private void OnDisable()
    {
        ladderActionRef.action.performed -= TryToClimb;
        ladderActionRef.action.canceled -= StopClimb;
    }

    private void TryToClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.Knockback)
            return;

        linkedAnimator.enabled = true;


        if (linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash
            || !canGoOnLadder || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;

        linkedStateMachine.ChangeState(PlayerStates.State.Ladders);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();
        climb = true;
        minimumClimbTime = setMinClimbTime;

    }

    private void StopClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;
        if (linkedStateMachine.currentState != PlayerStates.State.Ladders)
            return;

        linkedPhysics.ResetVelocity();
        linkedAnimator.enabled = false;
    }

    public override void ProcessAbility()
    {
        if (climb)
            minimumClimbTime -= Time.deltaTime;




        if (canGoOnLadder == false)
        {
            if (linkedPhysics.grounded == false)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
        }


        if (linkedPhysics.grounded && minimumClimbTime <= 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
    }

    public override void ProcessFixedAbility()
    {
        if (climb)
            linkedPhysics.rb.linearVelocity = new Vector2(0, linkedInput.verticalInput * climbSpeed);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(climbParameterInt, linkedStateMachine.currentState == PlayerStates.State.Ladders);
    }
}
