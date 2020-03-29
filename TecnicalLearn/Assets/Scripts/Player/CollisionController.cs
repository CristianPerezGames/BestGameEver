using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public float posLineDetectWall = 0.5f;
    public float distRay = 0.5f;
    public float timeClimp = 1f;
    public ContactFilter2D filter2D;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!playerController.InGround)
        {
            RaycastHit2D coll = GetColliderJumpCollision();
            if (coll.collider != null)
            {
                transform.position = Vector2.Lerp(transform.position, (Vector2)coll.point + new Vector2(0, 1f), Time.deltaTime * timeClimp);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    RaycastHit2D GetColliderJumpCollision()
    {
        Vector2 initLineCastGround = (Vector2)transform.position + new Vector2(0, posLineDetectWall);

        RaycastHit2D hit2DMidRigth = Physics2D.Raycast((Vector2)transform.position + new Vector2(0,-0.2f), transform.right, distRay + 0.1f, filter2D.layerMask);
        RaycastHit2D hit2DMidLefth = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, -0.2f), -transform.right, distRay + 0.1f, filter2D.layerMask);
        RaycastHit2D hit2D = Physics2D.Raycast(initLineCastGround, transform.right, distRay, filter2D.layerMask);

        if (hit2DMidRigth.collider == null)
        {
            if (hit2D)
            {
                return hit2D;
            }
        }
        
        if(hit2DMidLefth.collider == null)
        {
            Debug.Log("hit2DMidLefth");
            hit2D = Physics2D.Raycast(initLineCastGround, -transform.right, distRay, filter2D.layerMask);
            if (hit2D)
            {
                return hit2D;
            }
        }

        return hit2D;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0, posLineDetectWall), (Vector2)transform.position + new Vector2(distRay , posLineDetectWall));
        Gizmos.DrawLine((Vector2)transform.position , (Vector2)transform.position + new Vector2(distRay + 0.1f, 0));
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0, posLineDetectWall), (Vector2)transform.position + new Vector2(-distRay , posLineDetectWall));
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + new Vector2(-distRay - 0.1f, 0));
    }
}
