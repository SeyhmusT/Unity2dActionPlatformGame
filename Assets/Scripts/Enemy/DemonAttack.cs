using UnityEngine;

public class DemonAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<PlayerStats>().DamagePlayer(damage);

    }
}
