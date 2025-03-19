using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask layersToHit;

    private void Update()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 50f, layersToHit);
        if(hit.collider == null)
        {
            transform.localScale = new Vector3(50f, transform.localScale.y , 1);
            return;
        }

        transform.localScale = new Vector3(hit.distance, transform.localScale.y, 1);

        if(hit.collider.tag == "Player1" || hit.collider.tag == "Player2") 
        {
            Destroy(hit.collider.gameObject);
        }
    }
}
