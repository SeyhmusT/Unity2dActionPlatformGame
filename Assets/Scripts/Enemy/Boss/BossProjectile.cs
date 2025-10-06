using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.DamagePlayer(damage);
        }
        rb.linearVelocity = Vector2.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        anim.Play("BossProjectileExplosion");
    }

    public void MoveProjectile(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            rb.linearVelocity = Vector2.down * speed;
            return;
        }
        Vector3 targetPos = playerTransform.position + new Vector3(0, 1f, 0);
        Vector2 direction = (targetPos - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    public void Destroyer()
    {
        Destroy(gameObject);
    }
}
