using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PatrollingStateMachine : EnemySimpleStateMachine
{
    [SerializeField] private PatrolPhysics patrolPhysics;

    [Header("Idle State")]
    [SerializeField] private string idleAnimationName;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleStateTimer;

    [Header("Move State")]
    [SerializeField] private string moveAnimationName;
    [SerializeField] private float speed;
    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float minimumTurnDelay;
    private float moveStateTimer;
    private float turnCooldown;

    [Header("Attack State")]
    [SerializeField] private string attackAnimationName;

    [Header("Death State")]
    [SerializeField] private string deathAnimationName;

    #region IDLE
    public override void EnterIdle()
    {
        anim.Play(idleAnimationName);
        idleStateTimer = Random.Range(minIdleTime, maxIdleTime);
        patrolPhysics.NegateForces();
    }

    public override void UpdateIdle()
    {
        if (patrolPhysics.playerBehind)
        {
            ForceFlip();
            speed *= -1;
            turnCooldown = minimumTurnDelay;
            ChangeState(EnemyState.Move);
        }

        idleStateTimer -= Time.deltaTime;
        if (idleStateTimer <= 0 || patrolPhysics.playerAhead)
        {
            ChangeState(EnemyState.Move);
        }
        if (patrolPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    public override void ExitIdle()
    {
        // do something
    }
    #endregion

    #region  MOVE

    public override void EnterMove()
    {
        anim.Play(moveAnimationName);
        moveStateTimer = Random.Range(minMoveTime, maxMoveTime);
    }

    public override void UpdateMove()
    {
        moveStateTimer -= Time.deltaTime;
        if (moveStateTimer <= 0 && patrolPhysics.playerAhead == false)
            ChangeState(EnemyState.Idle);

        if (turnCooldown > 0)
            turnCooldown -= Time.deltaTime;

        if (patrolPhysics.playerBehind && turnCooldown <= 0)
        {
            ForceFlip();
            speed *= -1;
            turnCooldown = minimumTurnDelay;
            return;
        }

        if (patrolPhysics.wallDetected || patrolPhysics.groundDetected == false)
            {
                if (turnCooldown > 0)
                    return;

                ForceFlip();
                speed *= -1;
                turnCooldown = minimumTurnDelay;
            }

        if (patrolPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }

    }

    public override void FixedUpdateMove()
    {
        patrolPhysics.rb.linearVelocity = new Vector2(speed, patrolPhysics.rb.linearVelocityY);
    }


    #endregion

    #region ATTACK
    public override void EnterAttack()
    {
        anim.Play(attackAnimationName);
        patrolPhysics.NegateForces();
        patrolPhysics.canCheckBehind = false;
    }

    public void EndOfAttack()
    {
        if (patrolPhysics.inAttackRange)
        {
            anim.Play(attackAnimationName, 0, 0);
        }
        else
        {
            ChangeState(previousState);
        }
        StartCoroutine(CheckBehindDelay());
    }
    IEnumerator CheckBehindDelay()
    {
        yield return new WaitForSeconds(0.3f);
        patrolPhysics.canCheckBehind = true;
    }

    #endregion

    #region DEATH
    public override void EnterDeath()
    {
        anim.Play(deathAnimationName);
        patrolPhysics.DeathColliderDeactivation();
        patrolPhysics.NegateForces();
    }
    #endregion


}
