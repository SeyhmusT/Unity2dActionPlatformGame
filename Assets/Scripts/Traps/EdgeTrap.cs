using UnityEngine;

public class EdgeTrao : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = collision.GetComponent<PlayerStats>();
        stats.DamagePlayer(damage);
    }
}
