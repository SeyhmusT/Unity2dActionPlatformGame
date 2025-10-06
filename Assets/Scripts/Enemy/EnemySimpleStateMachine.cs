using UnityEngine;

public class EnemySimpleStateMachine : MonoBehaviour
{

    protected EnemyState previousState;
    protected EnemyState currentState;
    [SerializeField] protected Animator anim;
    public bool facingRight = true;

    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Death
    }
    public void ChangeState(EnemyState newState)
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
            case EnemyState.Idle:
                // do something like..
                UpdateIdle();
                break;
            case EnemyState.Attack:
                // do something like...
                UpdateAttack();
                break;
            case EnemyState.Move:
                // do something like...
                UpdateMove();
                break;
            case EnemyState.Death:
                //do something like...
                UpdateDeath();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                // do something like..
                FixedUpdateIdle();
                break;
            case EnemyState.Attack:
                // do something like...
                FixedUpdateAttack();
                break;
            case EnemyState.Move:
                // do something like...
                FixedUpdateMove();
                break;
            case EnemyState.Death:
                //do something like...
                FixedUpdateDeath();
                break;
        }
    }

    protected void EnterState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                // do something like..
                EnterIdle();
                break;
            case EnemyState.Attack:
                // do something like...
                EnterAttack();
                break;
            case EnemyState.Move:
                // do something like...
                EnterMove();
                break;
            case EnemyState.Death:
                //do something like...
                EnterDeath();
                break;

        }
    }

    protected void ExitState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                // do something like..
                ExitIdle();
                break;
            case EnemyState.Attack:
                // do something like...
                ExitAttack();
                break;
            case EnemyState.Move:
                // do something like...
                ExitMove();
                break;
            case EnemyState.Death:
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
    public virtual void EnterMove()
    {

    }
    public virtual void EnterDeath()
    {

    }


    // Exit States
    public virtual void ExitIdle()
    {

    }
    public virtual void ExitAttack()
    {

    }
    public virtual void ExitMove()
    {

    }
    public virtual void ExitDeath()
    {

    }

    // Update States
    public virtual void UpdateIdle()
    {

    }
    public virtual void UpdateAttack()
    {

    }
    public virtual void UpdateMove()
    {

    }
    public virtual void UpdateDeath()
    {

    }
    

    // Fixed Update States
     public virtual void FixedUpdateIdle()
    {

    }
    public virtual void FixedUpdateAttack()
    {

    }
    public virtual void FixedUpdateMove()
    {

    }
    public virtual void FixedUpdateDeath()
    {

    }
}
