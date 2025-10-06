using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;

    void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAbility knockbackAbility = collision.GetComponentInParent<KnockbackAbility>();
        knockbackAbility.StartKnockback(knockbackDuration, knockbackForce, transform.parent);
        collision.GetComponent<PlayerStats>().DamagePlayer(damage);
    }
}
