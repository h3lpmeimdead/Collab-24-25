using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    public GrapplingGun grapplingPlayer;
    public PlayerShooting shootingPlayer;

    void Start()
    {
        shootingPlayer.IsActive = true;
        grapplingPlayer.IsActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayers();
        }
    }

    void SwitchPlayers()
    {
        shootingPlayer.IsActive = !shootingPlayer.IsActive;
        grapplingPlayer.IsActive = !grapplingPlayer.IsActive;
    }
}
