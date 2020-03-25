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

    public float customGravity = 1;
    public float currGravity;
    void Start()
    {
        currGravity = customGravity;
    }

    void Update()
    {
        velocity = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && InGround())
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            CheckApplyCustomGravity();
        }
    }

    void CheckApplyCustomGravity()
    {
        if(Mathf.Abs(velocity) > 0.8f)
        {
            currGravity = customGravity;
        }
        else
        {
            currGravity = customGravity * 4;
        }
    }

    bool InGround()
    {
        if (Physics2D.Linecast(transform.position, transform.position + transform.up * -rayDistance, maskJumpLayer))
        {
            Debug.Log("In Ground");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(velocity * speed * Time.fixedDeltaTime, rb2D.velocity.y);

        if(rb2D.velocity.y < 10 && !InGround())
        {
            Debug.Log("Apply Gravity");
            rb2D.AddForce(-Vector2.up * currGravity, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * -rayDistance);
    }
}
