using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGoal : MonoBehaviour
{
    [SerializeField] private bool player1Ready = false;
    [SerializeField] private bool player2Ready = false;
    [SerializeField] private LayerMask player1Layer;
    [SerializeField] private LayerMask player2Layer;
    [SerializeField] private Color bothPlayersColor;
    [SerializeField] private Color onePlayerColor;
    [SerializeField] private Color defaultColor;
    [SerializeField] private int levelIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((player1Layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            player1Ready = true;
        }

        if ((player2Layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            player2Ready = true;
        }

        CheckBothPlayers();
        ChangeColor();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((player1Layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            player1Ready = false;
        }

        if ((player2Layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            player2Ready = false;
        }
        ChangeColor();
    }

    private void ChangeColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (player1Ready && player2Ready)
        {
            spriteRenderer.color = bothPlayersColor;
        }
        else if (player1Ready || player2Ready)
        {
            spriteRenderer.color = onePlayerColor;
        }
        else
        {
            spriteRenderer.color = defaultColor;
        }
    }

    private void CheckBothPlayers()
    {
        if (player1Ready && player2Ready)
        {
            SceneManager.LoadScene(levelIndex);
        }
    }
}
