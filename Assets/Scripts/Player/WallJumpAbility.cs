using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpAbility : BaseAbility
{
    public InputActionReference wallJumpActionRef;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallJumpMaxTime;
    private float wallJumpMinimumTime;
    private float wallJumpTimer;

    private void OnEnable()
    {
        wallJumpActionRef.action.performed += TryToWallJump;
    }

    void OnDisable()
    {
        wallJumpActionRef.action.performed -= TryToWallJump;
    }

    protected override void Initialization()
    {
        base.Initialization();
        wallJumpTimer = wallJumpMaxTime;
    }

    private void TryToWallJump(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.Knockback)
            return;
            

        if (EvaluateWallJumpConditions())
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallJump);
            wallJumpTimer = wallJumpMaxTime;
            wallJumpMinimumTime = 0.15f;
            // flip the player and then add velocity
            player.ForceFlip();

            if (player.facingRight)
                linkedPhysics.rb.linearVelocity = new Vector2(wallJumpForce.x, wallJumpForce.y);
            else
                linkedPhysics.rb.linearVelocity = new Vector2(-wallJumpForce.x, wallJumpForce.y);
        }
    }

    public override void ProcessAbility()
    {
        wallJumpTimer -= Time.deltaTime;
        wallJumpMinimumTime -= Time.deltaTime;

        if (wallJumpMinimumTime < 0 && linkedPhysics.grounded)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            return;
        }

        if (wallJumpTimer <= 0)
            {
                if (linkedPhysics.grounded)
                    linkedStateMachine.ChangeState(PlayerStates.State.Idle);
                else
                    linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                return;

            }

        
        if (wallJumpTimer <= 0 && linkedPhysics.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
            wallJumpTimer = -1;

        }

        
    }

    private bool EvaluateWallJumpConditions()
    {
        if (linkedPhysics.grounded || !linkedPhysics.wallDetected)
            return false;

        return true;
    }
}
