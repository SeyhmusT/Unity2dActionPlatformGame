using UnityEngine;

public class SwingBlade : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float maxAngle;
    [SerializeField] private float speed;
    private float timer;

    [Header("Knockback Settings")]
    [SerializeField] private float bladeDamage;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;


    private int pushDirection = 1;
    private float previousAngle = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += speed * Time.deltaTime;
        float angle = maxAngle * Mathf.Sin(timer);
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (angle > previousAngle)
            pushDirection = 1;
        else if (angle < previousAngle)
            pushDirection = -1;

        previousAngle = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAbility knockbackAbility = collision.GetComponentInParent<KnockbackAbility>();
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        
        // Oyuncu ölüyse hiçbir şey yapma
        if (playerStats != null && playerStats.GetCurrentHealth() <= 0)
            return;
            
        if (knockbackAbility != null)
            knockbackAbility.StartSwingKnockback(knockbackDuration, knockbackForce, pushDirection);

        if (playerStats != null)
            playerStats.DamagePlayer(bladeDamage);
    }
}
