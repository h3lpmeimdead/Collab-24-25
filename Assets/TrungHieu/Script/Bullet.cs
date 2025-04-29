using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Lifetime & Knockback")]
    public float lifeTime = 3f;
    public float knockbackForce = 10f;

    [Header("Collision Layers")]
    [Tooltip("Layers that the bullet should treat as walls and return to pool on collision.")]
    [SerializeField] private LayerMask wallLayers;

    private PlayerShooting shootingScript;

    private void Awake()
    {
        shootingScript = FindObjectOfType<PlayerShooting>();
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPoolOrDestroy), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPoolOrDestroy));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        int layer = other.layer;

        if ((wallLayers.value & (1 << layer)) != 0)
        {
            ReturnToPoolOrDestroy();
            return;
        }

        if (other.CompareTag("Player1"))
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            if (other.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
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
