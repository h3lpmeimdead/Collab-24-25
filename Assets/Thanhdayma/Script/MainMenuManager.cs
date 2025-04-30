using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel, _sfxSlider, _musicSlider;
    private bool _isPaused = false;

    private void Start()
    {
        _pausePanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bullet")
            Pause();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);
    }
    
    public void SFXSlider()
    {
        _sfxSlider.gameObject.SetActive(!_sfxSlider.gameObject.activeSelf);
        _musicSlider.SetActive(false);
    }

    public void MusicSlider()
    {
        _musicSlider.gameObject.SetActive(!_musicSlider.gameObject.activeSelf);
        _sfxSlider.SetActive(false);
    }
}
