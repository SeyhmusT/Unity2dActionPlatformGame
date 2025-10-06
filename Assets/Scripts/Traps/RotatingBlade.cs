using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] private float bladeDamage;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;


    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAbility knockbackAbility = collision.GetComponentInParent<KnockbackAbility>();
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        
        // Oyuncu ölüyse hiçbir şey yapma
        if (playerStats != null && playerStats.GetCurrentHealth() <= 0)
            return;
            
        if (knockbackAbility != null)
            knockbackAbility.StartKnockback(knockbackDuration, knockbackForce, transform);

        if (playerStats != null)
            playerStats.DamagePlayer(bladeDamage);
    }
}
