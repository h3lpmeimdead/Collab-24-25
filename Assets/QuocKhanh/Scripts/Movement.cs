using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveForce = new Vector2(horizontalInput * moveSpeed, 0f);
        rb.AddForce(moveForce, ForceMode2D.Force);
    }

}
