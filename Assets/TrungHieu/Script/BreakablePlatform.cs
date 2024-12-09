using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
   
    public GameObject breakEffect;  
    public AudioClip breakSound;   

    private AudioSource audioSource;

    private void Start()
    {
        
        if (breakSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = breakSound;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            if (breakEffect != null)
            {
                Instantiate(breakEffect, transform.position, Quaternion.identity);
            }

           
            if (audioSource != null)
            {
                audioSource.Play();
            }

            
            Destroy(gameObject, breakSound != null ? breakSound.length : 0f);

            
            Destroy(collision.gameObject);
        }
    }
}
