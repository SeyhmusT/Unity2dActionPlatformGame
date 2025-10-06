using UnityEngine;

public class Shell : MonoBehaviour
{

    private float verticalForce;
    private float horizontalForce;
    [SerializeField] private float minEjectForce;
    [SerializeField] private float maxEjectForce;
    [SerializeField] private float minHorizontalForce;
    [SerializeField] private float maxHorizontalForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float lifespan;
    void Start()
    {
        verticalForce = Random.Range(minEjectForce, maxEjectForce);
        horizontalForce = Random.Range(minHorizontalForce, maxHorizontalForce);
        Vector2 force = (Vector2)transform.up * verticalForce + (Vector2)transform.right * (-1) * horizontalForce;
        rb.AddForce(force, ForceMode2D.Impulse);
        Destroy(gameObject, lifespan);
    }

   
}
