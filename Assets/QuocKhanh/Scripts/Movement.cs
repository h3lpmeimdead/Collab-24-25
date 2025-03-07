using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private Rigidbody2D rb;

    private void FixedUpdate()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Horizontal * playerSpeed, rb.velocity.y);
    }
}
