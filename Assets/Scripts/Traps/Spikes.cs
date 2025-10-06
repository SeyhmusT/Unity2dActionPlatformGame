using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float spikeDamage;
    [SerializeField] private float knockbackDuration;
    [SerializeField] Vector2 knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAbility knockbackAbility = collision.GetComponentInParent<KnockbackAbility>();
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        
        // Oyuncu ölüyse hiçbir şey yapma
        if (playerStats != null && playerStats.GetCurrentHealth() <= 0)
            return;
            
        if (knockbackAbility != null)
            knockbackAbility.StartKnockback(knockbackDuration, knockbackForce, transform);
        //StartCoroutine(knockbackAbility.KnockBack(knockbackDuration, knockbackForce,transform));

        if (playerStats != null)
            playerStats.DamagePlayer(spikeDamage);
    }
}
