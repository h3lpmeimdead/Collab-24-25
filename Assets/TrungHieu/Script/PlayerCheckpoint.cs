using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector3 respawnPosition;

    private void Start()
    {
        
        respawnPosition = transform.position;
    }

    public void SetCheckpoint(Vector3 newCheckpointPosition)
    {
        
        respawnPosition = newCheckpointPosition;
    }

    public void Respawn()
    {
        transform.position = respawnPosition;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Respawn();
        }
    }
}
