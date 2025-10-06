using System.Collections;
using UnityEngine;

public class PatrollingRangeStateMachine : EnemySimpleStateMachine
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
    [SerializeField] private LineRenderer lineRender;
    [SerializeField] private float rayLength;
    [SerializeField] private float damage;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float visibleLineTime;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private LayerMask whatToHit;

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
        if (idleStateTimer <= 0 || patrolPhysics.playerAhead == false)
        {
            ChangeState(EnemyState.Move);
        }
        if (patrolPhysics.playerAhead)
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

        if (patrolPhysics.playerAhead)
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
        if (patrolPhysics.playerAhead)
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

    public void ShootAttack()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(shootingPoint.position, transform.right, rayLength, whatToHit);
        lineRender.positionCount = 2;
        if (hitInfo)
        {
            lineRender.SetPosition(0, shootingPoint.position);
            lineRender.SetPosition(1, hitInfo.point);
            Vector2 normal = hitInfo.normal;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(hitEffectPrefab, hitInfo.point, rotation);
            PlayerStats playerStats = hitInfo.collider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.DamagePlayer(damage);
            }
        }
        else
        {
            lineRender.SetPosition(0, shootingPoint.position);
            lineRender.SetPosition(1, shootingPoint.position + transform.right * 20);
        }
        StartCoroutine(ResetShootLine());
    }

    private IEnumerator ResetShootLine()
    {
        yield return new WaitForSeconds(visibleLineTime);
        lineRender.positionCount = 0;
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
