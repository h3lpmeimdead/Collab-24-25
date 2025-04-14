using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserButton : MonoBehaviour
{
    [SerializeField] private Laser laser;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            laser.ToggleLaser();
        }
    }
}
