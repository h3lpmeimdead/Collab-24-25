using UnityEngine;

public class NoGrapplingZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GrapplingGun grapple = other.gameObject.GetComponentInChildren<GrapplingGun>();
            if (grapple != null)
            {
                grapple.DisableGrappling();
            }
            else 
            {
                Debug.Log("not found");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             GrapplingGun grapple = other.gameObject.GetComponentInChildren<GrapplingGun>();
             if (grapple != null)
             {
                 grapple.EnableGrappling();
             }
            else
            {
                Debug.Log("not found");
            }
        }
    }
}
