using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public float shootingCooldown = 2f;
    public int poolSize = 10; // Number of projectiles to keep in the pool
    [SerializeField] private bool isActive = false;
   
    private Rigidbody2D rb;
    private float chargeTime;
    private bool isCharging;
    private bool canShoot = true;
    private float cooldownTimer;
    private Queue<GameObject> projectilePool; // Object pool
    private bool inShootingZone = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer; 
    private bool facingRight = true; 

    public bool rotateOverTime = true;

    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Movement Related")]
    [SerializeField] private bool isShooting;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float movementSpeed = 5f;
    public LayerMask groundLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        chargeBar.maxValue = maxKnockbackForce;
        chargeBar.minValue = minKnockbackForce;
        chargeBar.value = minKnockbackForce;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        InitializeProjectilePool();
    }

    private void FixedUpdate()
    {
        
        if (IsActive && (isGrounded || inShootingZone))
        {
            Movement();
        }
    }

    public void Die()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void Update()
    {
        if (!IsActive) return;
        CheckGrounded();
        UpdateChargeBarPosition();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateGun(mousePos, true);

        if (canShoot && !inShootingZone)
        {
            isShooting = false;
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

                FlipToMouse();
            }

            if (Input.GetMouseButtonUp(0) && isCharging)
            {
                isShooting = true;
                Shoot();
                isCharging = false;
                chargeBar.value = minKnockbackForce;
                StartCooldown();
            }
        }
        else
        {
            CooldownTimer();
        }
        if (!isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Cannon Jump"))
        {
            animator.Play("Cannon Jump");
        }
        // Handle aiming animations
        if (isCharging && isGrounded)
        {
            float moveInput = Mathf.Abs(Input.GetAxis("Horizontal"));

            if (moveInput > 0.1f)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cannon Gun Run"))
                {
                    animator.Play("Cannon Gun Run");
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cannon Aim"))
                {
                    animator.Play("Cannon Aim");
                }
            }
        }


    }
    public bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive == value) return;
            isActive = value;

            // Enable/disable the charge bar based on active state
            if (chargeBar != null)
            {
                chargeBar.gameObject.SetActive(isActive);
            }

            if (!isActive)
            {
                ResetCharge();
            }
        }
    }


    void ResetCharge()
    {
        isCharging = false;
        chargeBar.value = minKnockbackForce;
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

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, Vector2.down, 1.5f, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawRay(rb.transform.position, Vector2.down * 1.5f, Color.red);
    }

    private void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveInput * movementSpeed, rb.velocity.y);

        if (!isShooting)
        {
            rb.velocity = movement;
        }

        
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }


        if (animator != null)
        {
            if (Mathf.Abs(moveInput) > 0.1f)
            {
                if (isCharging)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cannon Run Gun"))
                    {
                        animator.Play("Cannon Run Gun");
                    }
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cannon Run"))
                    {
                        animator.Play("Cannon Run");
                    }
                }
            }
            else
            {
                if (isCharging)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cannon Aim"))
                    {
                        animator.Play("Cannon Aim");
                    }
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cannon Idle"))
                    {
                        animator.Play("Cannon Idle");
                    }
                }
            }
        }

    }
    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)shootingPoint.position - rb.position).normalized;

        if (direction.x < 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x > 0 && facingRight)
        {
            Flip();
        }
        //Get the bullet from the pool
        GameObject projectile = GetPooledProjectile();
        if (projectile != null)
        {
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = Quaternion.identity;
            projectile.SetActive(true);

            //Set projectile velocity based on charge time
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            float adjustedSpeed = Mathf.Lerp(projectileSpeed, projectileSpeed * 2, (chargeTime - minKnockbackForce) / (maxKnockbackForce - minKnockbackForce));
            projectileRb.velocity = direction * adjustedSpeed;

            // Apply knockback to the player
            rb.AddForce(-direction * chargeTime, ForceMode2D.Impulse);
        }
    }

    void StartCooldown()
    {
        canShoot = false;
        cooldownTimer = shootingCooldown;
    }

    void CooldownTimer()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            canShoot = true;
        }
    }

    public void UpdateChargeBarPosition()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + barOffset);
        chargeBar.transform.position = screenPosition;
    }

    void InitializeProjectilePool()
    {
        projectilePool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile); 
        }
    }

    GameObject GetPooledProjectile()
    {
        if (projectilePool.Count > 0)
        {
            GameObject projectile = projectilePool.Dequeue(); 
            return projectile; 
        }

        return null; // Pool is empty
    }

    public void ReturnProjectileToPool(GameObject projectile)
    {
        projectile.SetActive(false);
        projectilePool.Enqueue(projectile);
    }

    public void EnableShooting()
    {
        inShootingZone = false;
        Debug.Log("Shooting enabled.");
    }

    public void DisableShooting()
    {
        inShootingZone = true;
        Debug.Log("Shooting disabled.");
        StartCoroutine(EnableMovement());
    }
    private IEnumerator EnableMovement()
    {
       yield return new WaitForSeconds(1f);
       isShooting = false;
    }
    void Flip()
    {
        facingRight = !facingRight; 
        spriteRenderer.flipX = !facingRight;

        // Flip gunPivot and firePoint positions
        gunPivot.localPosition = new Vector3(-gunPivot.localPosition.x, gunPivot.localPosition.y, gunPivot.localPosition.z);
        firePoint.localPosition = new Vector3(-firePoint.localPosition.x, firePoint.localPosition.y, firePoint.localPosition.z);
    }
    void FlipToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseX = mouseWorldPos.x;

        if (mouseX < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (mouseX > transform.position.x && !facingRight)
        {
            Flip();
        }
    }


}
