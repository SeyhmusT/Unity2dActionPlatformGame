using UnityEngine;

public class BossStateMachine : MonoBehaviour
{

    protected BossState previousState;
    protected BossState currentState;
    [SerializeField] protected Animator anim;
    public bool facingRight = true;

    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
    public enum BossState
    {
        Idle,
        Teleport,
        Attack,
        RangeAttack,
        Summon,
        Death
    }
    public void ChangeState(BossState newState)
    {
        if (newState == currentState)
            return;

        ExitState(currentState);
        previousState = currentState;
        currentState = newState;
        EnterState(currentState);
    }

    private void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                // do something like..
                UpdateIdle();
                break;
            case BossState.Attack:
                // do something like...
                UpdateAttack();
                break;
            case BossState.Teleport:
                // do something like...
                UpdateTeleport();
                break;
            case BossState.RangeAttack:
                // do something like...
                UpdateRangeAttack();
                break;
            case BossState.Summon:
                // do something like...
                UpdateSummon();
                break;
            case BossState.Death:
                //do something like...
                UpdateDeath();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case BossState.Idle:
                // do something like..
                FixedUpdateIdle();
                break;
            case BossState.Attack:
                // do something like...
                FixedUpdateAttack();
                break;
            case BossState.Teleport:
                // do something like...
                FixedUpdateTeleport();
                break;
            case BossState.RangeAttack:
                // do something like...
                FixedUpdateRangeAttack();
                break;
            case BossState.Summon:
                // do something like...
                FixedUpdateSummon();
                break;
            case BossState.Death:
                //do something like...
                FixedUpdateDeath();
                break;
        }
    }

    protected void EnterState(BossState state)
    {
        switch (state)
        {
            case BossState.Idle:
                // do something like..
                EnterIdle();
                break;
            case BossState.Attack:
                // do something like...
                EnterAttack();
                break;
            case BossState.Teleport:
                // do something like...
                EnterTeleport();
                break;
            case BossState.RangeAttack:
                // do something like...
                EnterRangeAttack();
                break;
            case BossState.Summon:
                // do something like...
                EnterSummon();
                break;
            case BossState.Death:
                //do something like...
                EnterDeath();
                break;

        }
    }

    protected void ExitState(BossState state)
    {
        switch (state)
        {
            case BossState.Idle:
                // do something like..
                ExitIdle();
                break;
            case BossState.Attack:
                // do something like...
                ExitAttack();
                break;
            case BossState.Teleport:
                // do something like...
                ExitTeleport();
                break;
            case BossState.RangeAttack:
                // do something like...
                ExitRangeAttack();
                break;
            case BossState.Summon:
                // do something like...
                ExitSummon();
                break;
            case BossState.Death:
                //do something like...
                ExitDeath();
                break;

        }
    }
    // Enter States
    public virtual void EnterIdle()
    {

    }
    public virtual void EnterAttack()
    {

    }
    public virtual void EnterTeleport()
    {

    }
    public virtual void EnterDeath()
    {

    }
    public virtual void EnterRangeAttack()
    {

    }
    public virtual void EnterSummon()
    {

    }


    // Exit States
    public virtual void ExitIdle()
    {

    }
    public virtual void ExitAttack()
    {

    }
    public virtual void ExitTeleport()
    {

    }
    public virtual void ExitDeath()
    {

    }
    public virtual void ExitRangeAttack()
    {

    }
    public virtual void ExitSummon()
    {

    }

    // Update States
    public virtual void UpdateIdle()
    {

    }
    public virtual void UpdateAttack()
    {

    }
    public virtual void UpdateTeleport()
    {

    }
    public virtual void UpdateDeath()
    {

    }
    public virtual void UpdateRangeAttack()
    {

    }
    public virtual void UpdateSummon()
    {

    }


    // Fixed Update States
    public virtual void FixedUpdateIdle()
    {

    }
    public virtual void FixedUpdateAttack()
    {

    }
    public virtual void FixedUpdateTeleport()
    {

    }
    public virtual void FixedUpdateDeath()
    {

    }
    public virtual void FixedUpdateRangeAttack()
    {

    }
    public virtual void FixedUpdateSummon()
    {

    }
}
