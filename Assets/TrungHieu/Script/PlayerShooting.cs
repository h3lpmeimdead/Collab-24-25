using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;  
    public Transform shootingPoint;      
    public float projectileSpeed = 10f;  
    public float knockbackForce = 5f;    
    public float shootingPointDistance = 1f; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RotateShootingPoint();

        if (Input.GetMouseButtonDown(0))  
        {
            Shoot();
        }
    }

    void RotateShootingPoint()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        shootingPoint.position = (Vector2)transform.position + direction * shootingPointDistance;
    }

    void Shoot()
    {
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - shootingPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = direction * projectileSpeed;
        rb.AddForce(-direction * knockbackForce, ForceMode2D.Impulse);
    }
}
