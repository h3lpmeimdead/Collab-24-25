using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGrapplingZone : MonoBehaviour
{
    [SerializeField] GrapplingGun grapplingGun;
    [SerializeField] Rope rope;
    private void OnTriggerStay2D (Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            grapplingGun.enabled = false;
            rope.enabled = false;
            grapplingGun.m_springJoint2D.enabled = false;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            grapplingGun.enabled = true;
            rope.enabled = true;
            grapplingGun.m_springJoint2D.enabled = true;
        }
        else
        {
            return;
        }
    }

}
