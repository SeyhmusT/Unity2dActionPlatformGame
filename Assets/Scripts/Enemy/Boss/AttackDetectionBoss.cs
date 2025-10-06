using UnityEngine;

public class AttackDetectionBoss : MonoBehaviour
{
    [SerializeField] private BossPhysics bossPhysics;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bossPhysics.inAttackRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bossPhysics.inAttackRange = false;
    }
}
