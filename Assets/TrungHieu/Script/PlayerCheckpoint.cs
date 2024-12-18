using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector3 respawnPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        respawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetCheckpoint(Vector3 newCheckpointPosition)
    {
        respawnPosition = newCheckpointPosition;
    }

    public void Respawn()
    {
        
        transform.position = respawnPosition;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f; 
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.CompareTag("Wall"))
        //{
        //    Respawn();
        //}
    }
}
