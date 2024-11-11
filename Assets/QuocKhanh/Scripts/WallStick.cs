using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStick : MonoBehaviour
{
    public float wallCheckDistance = 0.5f;
    public float stickForce = 5f;
    public LayerMask wallLayer;
    private Rigidbody2D rb;
    public bool isSticking;

    void Start()
    {
        isSticking = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        WallCheck();
    }

    void WallCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallCheckDistance, wallLayer);

        if (hit.collider)
        {
            StickToWall();
        }
        else
        {
            isSticking = false;
        }
    }

    void StickToWall()
    {
        isSticking = true;
        rb.velocity = new Vector2(rb.velocity.x, stickForce);
    }
}
