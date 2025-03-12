using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerShooting shootingScript;
    public float lifeTime = 5f;
    public float knockbackForce = 10f; // Adjustable knockback force

    private void Start()
    {
        shootingScript = FindObjectOfType<PlayerShooting>();

        // Schedule destruction of the bullet after its lifetime
        Invoke(nameof(ReturnToPoolOrDestroy), lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ReturnToPoolOrDestroy();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
           
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

            
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }

            // Destroy or return the bullet to the pool
            ReturnToPoolOrDestroy();
        }
    }

    private void ReturnToPoolOrDestroy()
    {
        if (shootingScript != null)
        {
            shootingScript.ReturnProjectileToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
