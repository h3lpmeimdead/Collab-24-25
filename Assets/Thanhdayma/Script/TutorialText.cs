using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TMP_Text textToFade;
    [Range(0f, 5f)] [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        if (textToFade != null)
        {
            // Set alpha to 0 at the beginning
            Color color = textToFade.color;
            color.a = 0f;
            textToFade.color = color;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI reference not set in " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textToFade != null)
        {
            textToFade.DOFade(1f, fadeDuration);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textToFade != null)
        {
            textToFade.DOFade(0f, fadeDuration);
        }
    }
}
