using UnityEngine;

public class Clone : MonoBehaviour, IDamageable
{
    public float health;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
