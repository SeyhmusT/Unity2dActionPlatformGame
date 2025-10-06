using UnityEngine;

public class WallSlideAbility : BaseAbility
{
    [SerializeField] private float maxSlideSpeed;
    private string wallSlideAnimParameterName = "WallSlide";
    private int wallSlideParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        wallSlideParameterID = Animator.StringToHash(wallSlideAnimParameterName);
    }

    public override void EnterAbility()
    {
        linkedPhysics.rb.linearVelocity = Vector2.zero;
        player.DeactivateCurrentWeapon();
    }
    public override void ExitAbility()
    {
        player.ActivateCurrentWeapon();
    }
    public override void ProcessAbility()
    {
        if (linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            return;
        }
        if (player.facingRight && linkedInput.horizontalInput < 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.wallDetected = false;
            linkedAnimator.SetBool("WallSlide", false);
            return;
        }
        if (!player.facingRight && linkedInput.horizontalInput > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.wallDetected = false;
            linkedAnimator.SetBool("WallSlide", false);
            return;
        }
        if (!linkedPhysics.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }
    }

    public override void ProcessFixedAbility()
    {
        linkedPhysics.rb.linearVelocityY = Mathf.Clamp(linkedPhysics.rb.linearVelocityY, -maxSlideSpeed, 1);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(wallSlideParameterID, linkedStateMachine.currentState == PlayerStates.State.WallSlide);
    }
}
