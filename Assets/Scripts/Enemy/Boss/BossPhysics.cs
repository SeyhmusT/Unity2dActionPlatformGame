using UnityEngine;

public class BossPhysics : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private PolygonCollider2D statsCol;
    [SerializeField] private PolygonCollider2D attackCol;
    [SerializeField] private BoxCollider2D attackDetectionCol;
    public bool inAttackRange;

    public void EnableStatsCol()
    {
        statsCol.enabled = true;
    }

    public void DisableStatsCol()
    {
        statsCol.enabled = false;
    }

    public void EnableAttackCol()
    {
        attackCol.enabled = true;
    }
    public void DisableAttackCol()
    {
        attackCol.enabled = false;
    }
    public void EnableDetectionCol()
    {
        attackDetectionCol.enabled = true;
    }
    public void DisableDetectionCol()
    {
        attackDetectionCol.enabled = false;
    }
    public void DisableAllColliders()
    {
        DisableAttackCol();
        DisableDetectionCol();
        DisableStatsCol();
    }
}
