using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int startIndex;
    [SerializeField] private Transform[] points;
    private Player player;
    private int targetIndex;
    void Start()
    {
        targetIndex = startIndex;
        transform.position = points[targetIndex].position;
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[targetIndex].position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, points[targetIndex].position) < 0.05f)
        {
            targetIndex++;
            if (targetIndex == points.Length)
                targetIndex = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    collision.transform.SetParent(this.transform);
                    player = collision.gameObject.GetComponent<Player>();
                    player.physicsControl.SetExtrapolate();
                    break;
                }
            }
            
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy == true)
            {
                collision.transform.SetParent(null);
                if(player!=null)
                    player.physicsControl.SetInterpolate();
                player = null;
            }
        }
    }
}
