using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class MultipleJumpAbility : BaseAbility
{
    public InputActionReference jumpActionRef;

    [SerializeField] private int maxNumberOfJumps;
    private int numberOfJumps;
    private bool canActivateAdditionalJumps;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;
    [SerializeField] private float minimumAirTime;
    private float startMinimumAirTime;

    [SerializeField] private float setMaxJumpTime;
    private float jumpTimer;
    private bool jumping;
    [SerializeField] private float gravityDivider;

    private string jumpAnimParameterName = "Jump";
    private string ySpeedAnimParameterName = "ySpeed";
    private int jumpParameterID;
    private int ySpeedParameterID;
    private PlayerInput playerInput;

    protected override void Initialization()
    {
        base.Initialization();
        startMinimumAirTime = minimumAirTime;
        numberOfJumps = maxNumberOfJumps;
        jumpParameterID = Animator.StringToHash(jumpAnimParameterName);
        ySpeedParameterID = Animator.StringToHash(ySpeedAnimParameterName);
        playerInput = player != null ? player.GetComponent<PlayerInput>() : null;
    }
    private void OnEnable()
    {

        jumpActionRef.action.Enable();
        jumpActionRef.action.performed += tryToJump;
        jumpActionRef.action.canceled += StopJump;
    }
    void OnDisable()
    {

        jumpActionRef.action.Disable();
        jumpActionRef.action.performed -= tryToJump;
        jumpActionRef.action.canceled -= StopJump;

    }

    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirTime -= Time.deltaTime;

        if (jumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
            {
                jumping = false;
            }
        }

        if (linkedPhysics.grounded && minimumAirTime < 0)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }

        if (!linkedPhysics.grounded && linkedPhysics.wallDetected)
        {
            if (linkedPhysics.rb.linearVelocityY < 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
            }
        }
    }

    public override void ProcessFixedAbility()
    {
        if (!linkedPhysics.grounded)
        {
            if (jumping)
                linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce);
            else
                linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, Mathf.Clamp(linkedPhysics.rb.linearVelocityY, -10, jumpForce));

        }
        if (linkedPhysics.rb.linearVelocityY < 0)
        {
            linkedPhysics.rb.gravityScale = linkedPhysics.GetGravity() / gravityDivider;
        }
    }
    private void tryToJump(InputAction.CallbackContext value)
    {
        if (playerInput != null && playerInput.currentActionMap != null && playerInput.currentActionMap.name != "Player" || !isPermitted || linkedStateMachine.currentState == PlayerStates.State.Knockback || linkedStateMachine.currentState == PlayerStates.State.Death
        || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, 0);
            minimumAirTime = startMinimumAirTime;
            jumping = true;
            jumpTimer = setMaxJumpTime;
            numberOfJumps = maxNumberOfJumps;
            canActivateAdditionalJumps = true;
            numberOfJumps -= 1;
            source.PlayOneShot(clip);
            return;
        }

        if (linkedPhysics.coyoteTimer > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce);
            minimumAirTime = startMinimumAirTime;
            linkedPhysics.coyoteTimer = -1;
            jumping = true;
            jumpTimer = setMaxJumpTime;
            numberOfJumps = maxNumberOfJumps;
            canActivateAdditionalJumps = true;
            numberOfJumps -= 1;
            source.PlayOneShot(clip);
            return;
        }
        if (numberOfJumps > 0 && canActivateAdditionalJumps)
        {
            linkedPhysics.EnableGravity();
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce);
            minimumAirTime = startMinimumAirTime;
            linkedPhysics.coyoteTimer = -1;
            jumping = true;
            jumpTimer = setMaxJumpTime;
            numberOfJumps -= 1;
            source.PlayOneShot(clip);
        }
        else
        {
            canActivateAdditionalJumps = false;
        }


    }
    private void StopJump(InputAction.CallbackContext value)
    {
        jumping = false;
    }
    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        canActivateAdditionalJumps = false;
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParameterID, linkedStateMachine.currentState == PlayerStates.State.Jump || linkedStateMachine.currentState == PlayerStates.State.WallJump);
        linkedAnimator.SetFloat(ySpeedParameterID, linkedPhysics.rb.linearVelocityY);
    }

    public void SetMaxJumpNumber(int maxJumps)
    {
        maxNumberOfJumps = maxJumps;
    }
}
