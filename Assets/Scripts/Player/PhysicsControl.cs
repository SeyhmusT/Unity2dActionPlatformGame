using UnityEngine;

public class PhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteSetTime;
    public float coyoteTimer;



    [Header("Ground")]
    [SerializeField] private float gorundRayDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private LayerMask whatDoDetect;
    public bool grounded;
    private RaycastHit2D hitInfoLeft;
    private RaycastHit2D hitInfoRight;

    [Header("Wall")]
    [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform wallCheckPointUpper;
    [SerializeField] private Transform wallCheckPointLower;
    public bool wallDetected;
    private RaycastHit2D hitInfoWallUpper;
    private RaycastHit2D hitInfoWallLower;


    [Header("Ceiling")]
    [SerializeField] private float ceilingRayDistance;
    [SerializeField] private Transform ceilingCheckPointLeft;
    [SerializeField] private Transform ceilingCheckPointRight;
    public bool ceilingDetected;
    private RaycastHit2D hitInfoCeilingLeft;
    private RaycastHit2D hitInfoCeilingRight;




    [Header("Colliders")]
    [SerializeField] private Collider2D standColl;
    [SerializeField] private Collider2D crouchColl;

    [Header("Interpolation")]
    public RigidbodyInterpolation2D interpolate;
    public RigidbodyInterpolation2D extrapolate;


    private float gravityValue;

    public float GetGravity()
    {
        return gravityValue;
    }

    void Start()
    {
        gravityValue = rb.gravityScale;
        coyoteTimer = coyoteSetTime;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ceilingCheckPointLeft.position, new Vector3(0, ceilingRayDistance, 0));
        Debug.DrawRay(ceilingCheckPointRight.position, new Vector3(0, ceilingRayDistance, 0));

    }

    public void SetInterpolate()
    {
        rb.interpolation = interpolate;
    }
    public void SetExtrapolate()
    {
        rb.interpolation = extrapolate;
    }

    private bool CheckCeiling()
    {
        hitInfoCeilingLeft = Physics2D.Raycast(ceilingCheckPointLeft.position, Vector2.up, ceilingRayDistance, whatDoDetect);
        hitInfoCeilingRight = Physics2D.Raycast(ceilingCheckPointRight.position, Vector2.up, ceilingRayDistance, whatDoDetect);




        if (hitInfoCeilingLeft || hitInfoCeilingRight)
            return true;

        return false;
    }


    private bool CheckWall()
    {
        hitInfoWallUpper = Physics2D.Raycast(wallCheckPointUpper.position, transform.right, wallRayDistance, whatDoDetect);
        hitInfoWallLower = Physics2D.Raycast(wallCheckPointLower.position, transform.right, wallRayDistance, whatDoDetect);

        

        if (hitInfoWallLower || hitInfoWallUpper)
            return true;

        return false;

    }

    public void StandColliders()
    {
        standColl.enabled = true;
        crouchColl.enabled = false;
    }

    public void CrouchColliders()
    {
        standColl.enabled = false;
        crouchColl.enabled = true;
    }
    

    private bool CheckGround()
    {
        hitInfoLeft = Physics2D.Raycast(leftGroundPoint.position, Vector2.down, gorundRayDistance, whatDoDetect);
        hitInfoRight = Physics2D.Raycast(rightGroundPoint.position, Vector2.down, gorundRayDistance, whatDoDetect);

        Debug.DrawRay(leftGroundPoint.position, new Vector3(0, -gorundRayDistance, 0), Color.blue);
        Debug.DrawRay(rightGroundPoint.position, new Vector3(0, -gorundRayDistance, 0), Color.blue);


        if (hitInfoLeft || hitInfoRight)
            return true;

        return false;

    }


    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityValue;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (!grounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = coyoteSetTime;
        }

    }

    private void FixedUpdate()
    {
        grounded = CheckGround();
        wallDetected = CheckWall();
        ceilingDetected = CheckCeiling();
    }
}
