using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class JumpAbility : BaseAbility
{
    public InputActionReference jumpActionRef;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;
    [SerializeField] private float minimumAirTime;
    private float startMinimumAirTime;

    private string jumpAnimParameterName = "Jump";
    private string ySpeedAnimParameterName = "ySpeed";
    private int jumpParameterID;
    private int ySpeedParameterID;

    private PlayerInput playerInput;

    protected override void Initialization()
    {
        base.Initialization();
        startMinimumAirTime = minimumAirTime;
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
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, linkedPhysics.rb.linearVelocityY);
        }
    }
    private void tryToJump(InputAction.CallbackContext value)
    {


        if (playerInput != null && playerInput.currentActionMap != null && playerInput.currentActionMap.name != "Player" || !isPermitted || linkedStateMachine.currentState == PlayerStates.State.Knockback)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, 0);
            minimumAirTime = startMinimumAirTime;
            return;
        }

        if (linkedPhysics.coyoteTimer > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce);
            minimumAirTime = startMinimumAirTime;
            linkedPhysics.coyoteTimer = -1;
        }


    }
    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("STOP JUMP");
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParameterID, linkedStateMachine.currentState == PlayerStates.State.Jump || linkedStateMachine.currentState == PlayerStates.State.WallJump);
        linkedAnimator.SetFloat(ySpeedParameterID, linkedPhysics.rb.linearVelocityY);
    }
}
