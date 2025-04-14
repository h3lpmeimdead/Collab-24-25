using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] LayerMask player1Layer;
    [SerializeField] LayerMask player2Layer;
    private Rigidbody2D rb;
    private bool isDead = false;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float pointReachedThreshold = 0.1f;
    private int currentTargetIndex = 0;
    private int direction = 1;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (patrolPoints.Length > 0)
        {
            currentTargetIndex = 0;
        }
    }

    void FixedUpdate()
    {
        if (!isDead && patrolPoints.Length > 0)
        {
            PatrolMovement();
        }
    }

    private void PatrolMovement()
    {
        Vector2 targetPosition = patrolPoints[currentTargetIndex].position;
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;


        rb.velocity = new Vector2(moveDirection.x * patrolSpeed, rb.velocity.y);


        if (Vector2.Distance(transform.position, targetPosition) < pointReachedThreshold)
        {
            FlipSprite();
            UpdateTargetIndex();
        }
    }

    private void UpdateTargetIndex()
    {
        currentTargetIndex += direction;

        if (currentTargetIndex >= patrolPoints.Length || currentTargetIndex < 0)
        {
            direction *= -1;
            currentTargetIndex += direction * 2;
        }
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((player1Layer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerShooting player1 = other.GetComponent<PlayerShooting>();
            if (player1 != null)
            {
                player1.Die();
            }
        }
        if ((player2Layer.value & (1 << other.gameObject.layer)) != 0)
        {
            GrapplingGun player2 = other.GetComponent<GrapplingGun>();
            if (player2 != null)
            {
                player2.Die();
            }
        }
        if (other.CompareTag("HeavyObject"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.y < 0)
            {
                TriggerDeath();
            }
        }
    }

    public void TriggerDeath()
    {
        if (!isDead)
        {
            StartCoroutine(DeathAnimation());
            isDead = true;
        }
    }

    private IEnumerator DeathAnimation()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        

       
        rb.gravityScale = 3f;
        rb.velocity = new Vector2(0, 5f);

        
        StartCoroutine(SpinSprite());

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private IEnumerator SpinSprite()
    {
        float spinSpeed = 360f;
        while (true)
        {
            transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
