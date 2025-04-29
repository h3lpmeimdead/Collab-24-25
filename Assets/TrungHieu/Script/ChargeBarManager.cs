using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarManager : MonoBehaviour
{
    private PlayerShooting playerShooting;

    void Start()
    {
        playerShooting = FindObjectOfType<PlayerShooting>();
    }
}
