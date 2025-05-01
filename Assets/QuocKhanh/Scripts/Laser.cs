using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask layersToHit;
    private bool isActive = true;
    private void Update()
    {
        if (!isActive)
        {
            transform.localScale = new Vector3(0, transform.localScale.y, 1);
            return;
        }
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1000, layersToHit);
        if(hit.collider == null)
        {
            transform.localScale = new Vector3(50f, transform.localScale.y , 1);
            return;
        }

        transform.localScale = new Vector3(hit.distance, transform.localScale.y, 1);

        if(hit.collider.tag == "Player") 
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
    public void ToggleLaser()
    {
        isActive = !isActive;
    }
}
