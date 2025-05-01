using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    [SerializeField] private GameObject _quitPanel;

    private void Start()
    {
        _quitPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _quitPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _quitPanel.SetActive(false);
    }
}
