using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public void OpenDoor()
    {
        isOpen = true;
        StartCoroutine(OpenAnimation());
    }

    private IEnumerator OpenAnimation()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up * 9f; 

        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        GetComponent<Collider2D>().enabled = false;
    }

}
