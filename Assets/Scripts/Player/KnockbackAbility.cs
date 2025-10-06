using System.Collections;
using UnityEngine;

public class KnockbackAbility : BaseAbility
{
    private Coroutine currentKnockback;

    public override void ExitAbility()
    {
        currentKnockback = null;
    }
     public void StartSwingKnockback(float duration, Vector2 force, int direction)
    {
        if (player.playerStats.GetCanTakeDamage() == false)
            return;      

        if (currentKnockback == null)
            {
                currentKnockback = StartCoroutine(SwingKnockBack(duration, force, direction));
            }
            else
            {
                // do nothing
            }
    }

    public void StartKnockback(float duration, Vector2 force, Transform enemyObject)
    {
        if (player.playerStats.GetCanTakeDamage() == false)
            return;

        if (currentKnockback == null)
        {
            currentKnockback = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
        else
        {
            // do nothing
        }
    }
    public IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        linkedStateMachine.ChangeState(PlayerStates.State.Knockback);
        linkedPhysics.ResetVelocity();
        if (transform.position.x >= enemyObject.transform.position.x)
            linkedPhysics.rb.linearVelocity = force;
        else
            linkedPhysics.rb.linearVelocity = new Vector2(-force.x, force.y);

        yield return new WaitForSeconds(duration);
        // return for other states

        if (player.playerStats.GetCurrentHealth() > 0)
        {
            if (linkedPhysics.grounded)
            {
                if (linkedInput.horizontalInput != 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Run);
                }
                else
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Idle);
                }
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }

        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Death);
        }
    }

    public IEnumerator SwingKnockBack(float duration, Vector2 force, int direction)
    {
        linkedStateMachine.ChangeState(PlayerStates.State.Knockback);
        linkedPhysics.ResetVelocity();
        force.x *= direction;
        linkedPhysics.rb.linearVelocity = force;

        yield return new WaitForSeconds(duration);
        // return for other states

        if (player.playerStats.GetCurrentHealth() > 0)
        {
            if (linkedPhysics.grounded)
            {
                if (linkedInput.horizontalInput != 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Run);
                }
                else
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Idle);
                }
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }

        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Death);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool("Knockback", linkedStateMachine.currentState == PlayerStates.State.Knockback);
    }
   
}
