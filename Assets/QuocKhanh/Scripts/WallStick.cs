using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStick : MonoBehaviour
{
    [SerializeField] private bool isWallSliding;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallSlideLayer;
    [SerializeField] private float wallSlidingSpeed = 1.5f;

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 1.2f, wallSlideLayer);
    }

    private void WallSlide()
    {
        if(IsWalled())
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void Update()
    {
        WallSlide();
    }
}
