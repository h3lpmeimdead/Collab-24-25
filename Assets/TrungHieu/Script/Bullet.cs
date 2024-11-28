using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerShooting shootingScript;

    private void Start()
    {
        shootingScript = FindObjectOfType<PlayerShooting>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
           
            shootingScript.ReturnProjectileToPool(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        
        if (shootingScript != null)
        {
            shootingScript.ReturnProjectileToPool(gameObject);
        }
    }
}
