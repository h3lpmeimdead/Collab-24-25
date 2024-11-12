using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    public GrapplingGun player1Controller;

    public GrapplingGun player2Controller;
    public LineRenderer player2LineRenderer;
    public bool player1Active = true;

    private void Start()
    {
        player2Controller.enabled = false;
        player2LineRenderer.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) 
        {
            Switch();
        }
    }

    public void Switch()
    {
        if (player1Active) 
        {
            player1Controller.enabled = false;

            player2Controller.enabled = true;

            player1Active = false;
        }
        else
        {
            player1Controller.enabled = true;

            player2Controller.enabled = false;

            player1Active = true;
        }
    }
}
