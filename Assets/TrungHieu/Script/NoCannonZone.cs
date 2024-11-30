using UnityEngine;

public class NoCannonZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                playerShooting.DisableShooting(); 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                playerShooting.EnableShooting(); 
            }
        }
    }
}
