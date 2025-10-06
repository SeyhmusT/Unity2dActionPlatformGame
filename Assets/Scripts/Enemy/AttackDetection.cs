using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    [SerializeField] private PatrolPhysics patrolPhysics;
    private void OnTriggerStay2D(Collider2D collision)
    {
        patrolPhysics.inAttackRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        patrolPhysics.inAttackRange = false;
    }
}
