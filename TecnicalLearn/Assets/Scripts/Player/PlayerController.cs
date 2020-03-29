using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float velocity;
    public float speed = 10;
    public float jumpForce = 10;
    public LayerMask maskJumpLayer;
    public float rayDistance = 1;
    public bool doobleJump = false;

    public float customGravity = 1;
    public float currGravity;
    public float lateralRaysGorund = 1f;

    public bool InGround { get; private set; }

    void Start()
    {
        currGravity = customGravity;
    }

    void Update()
    {
        InGround = InGroundRay();
        velocity = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (InGround)
            {
                JumpPlayer();
                doobleJump = true;
            }else if(doobleJump)
            {
                doobleJump = false;
                JumpPlayer();
            }
        }
    }

    void JumpPlayer()
    {
        CheckApplyCustomGravity();
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Force);
    }

    void CheckApplyCustomGravity()
    {
        currGravity = customGravity * (1.2f - Mathf.Abs(velocity));
    }

    bool InGroundRay()
    {
        Vector2 posInitRay = new Vector2(lateralRaysGorund, 0);

        RaycastHit2D hit2DCenter = Physics2D.Raycast(transform.position, -transform.up, rayDistance, maskJumpLayer);
        RaycastHit2D hit2DLeft = Physics2D.Raycast((Vector2)transform.position + posInitRay, -transform.up, rayDistance, maskJumpLayer);
        RaycastHit2D hit2DRigth = Physics2D.Raycast((Vector2)transform.position - posInitRay, -transform.up, rayDistance, maskJumpLayer);

        if (hit2DCenter.collider || hit2DLeft.collider || hit2DRigth.collider)
        {
            Debug.Log("In Ground");
            doobleJump = true;

            if (hit2DCenter.collider)
                transform.up = hit2DCenter.normal;
            if (hit2DLeft.collider)
                transform.up = hit2DLeft.normal;
            if (hit2DRigth.collider)
                transform.up = hit2DRigth.normal;

            return true;
        }
        else
        {
            transform.up = Vector2.up;
            return false;
        }
    }

    private void FixedUpdate()
    {   
        rb2D.velocity = new Vector2(velocity * speed * Time.fixedDeltaTime, rb2D.velocity.y);

        if(rb2D.velocity.y < 10 && !InGround)
        {
            Debug.Log("Apply Gravity");
            rb2D.AddForce(-Vector2.up * currGravity, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * -rayDistance);

        Vector2 posInitRay = new Vector2(lateralRaysGorund, 0);
        Gizmos.DrawLine((Vector2)transform.position + posInitRay, (Vector2)transform.position + posInitRay + (Vector2)transform.up * -rayDistance);
        Gizmos.DrawLine((Vector2)transform.position - posInitRay, (Vector2)transform.position - posInitRay + (Vector2)transform.up * -rayDistance);
    }
}
