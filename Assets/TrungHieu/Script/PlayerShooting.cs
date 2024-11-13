using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float projectileSpeed = 10f;
    public float maxKnockbackForce = 15f;
    public float minKnockbackForce = 5f;
    public float shootingPointDistance = 1f;
    public Slider chargeBar;  
    public Vector3 barOffset = new Vector3(0, 0.5f, 0);
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

    private Rigidbody2D rb;
    private float chargeTime;
    private bool isCharging;
    private Vector2 respawnPosition;
    public bool rotateOverTime = true;

    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        chargeBar.maxValue = maxKnockbackForce;
        chargeBar.minValue = minKnockbackForce;
        chargeBar.value = minKnockbackForce;
    }

    void Update()
    {
        
        UpdateChargeBarPosition();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateGun(mousePos, true);

        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            chargeTime = minKnockbackForce;
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            chargeTime += Time.deltaTime * (maxKnockbackForce - minKnockbackForce);
            chargeTime = Mathf.Clamp(chargeTime, minKnockbackForce, maxKnockbackForce);
            chargeBar.value = chargeTime;
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            Shoot();
            isCharging = false;
            chargeBar.value = minKnockbackForce;
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - shootingPoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = direction * projectileSpeed;

        rb.AddForce(-direction * chargeTime, ForceMode2D.Impulse);
    }

    void UpdateChargeBarPosition()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + barOffset);
        chargeBar.transform.position = screenPosition;
    }
}
