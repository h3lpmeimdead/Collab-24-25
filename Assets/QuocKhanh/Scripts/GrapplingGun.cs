using UnityEngine;
using TMPro;
using System.Collections;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Rope grappleRope;

    [Header("UI Settings:")]
    public TMP_Text outOfRangeText;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int playerLayer = 11;
    [SerializeField] private int grappleableLayer = 9;
    [SerializeField] private int slideLayer = 10;
    [SerializeField] private bool inGrapplingZone = false;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] public bool isGrappling;
    [SerializeField] public bool isGrounded;
    public LayerMask groundLayer;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        isGrappling = false;
        if (outOfRangeText != null)
            outOfRangeText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isGrappling && isGrounded)
        {
            Movement();
        }
    }

    private void Update()
    {

        CheckGrounded();
        if (Input.GetKeyDown(KeyCode.Mouse0) && inGrapplingZone == false)
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            isGrappling = true;
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
            isGrappling = false;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }
    }

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_rigidbody.transform.position, Vector2.down, 1.2f, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
         Debug.DrawRay(m_rigidbody.transform.position, Vector2.down * 1.2f, Color.red);
    }

    private void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveInput * movementSpeed, m_rigidbody.velocity.y);
        m_rigidbody.velocity = movement;
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

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, distanceVector.normalized, hasMaxDistance ? maxDistnace : Mathf.Infinity);

        foreach (RaycastHit2D hit in hits)
        {
            if (grappleToAll || (1 << hit.collider.gameObject.layer & (1 << playerLayer | 1 << grappleableLayer | 1 << slideLayer)) != 0)
            {
                grapplePoint = hit.point;
                grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                grappleRope.enabled = true;
                return;
            }
        }
        grappleRope.enabled = false;

        if (hasMaxDistance)
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            float distanceToMouse = Vector2.Distance(firePoint.position, mousePos);

            if (distanceToMouse > maxDistnace || hits == null)
            {
                if (outOfRangeText != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(ShowOutOfRangeMessage());
                }
            }
        }
    }

    private IEnumerator ShowOutOfRangeMessage()
    {
        outOfRangeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        outOfRangeText.gameObject.SetActive(false);
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }
            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

    public void EnableGrappling()
    {
        inGrapplingZone = false;
    }

    public void DisableGrappling()
    {
        inGrapplingZone = true;
    }
}
