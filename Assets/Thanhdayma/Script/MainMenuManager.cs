using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel, _sfxSlider, _musicSlider, _tutorialPanel, _quitPanel, _playPanel;
    private bool _isPaused = false;
    [SerializeField] private int _index;

    private void Start()
    {
        _pausePanel.SetActive(false);
        _tutorialPanel.SetActive(false);
        _quitPanel.SetActive(false);
        _playPanel.SetActive(false);
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
        _tutorialPanel.SetActive(false);
        _playPanel.SetActive(false);
        _quitPanel.SetActive(false);
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

    public void Tutorial()
    {
        _tutorialPanel.SetActive(true);
        _pausePanel.SetActive(false);
        _musicSlider.SetActive(false);
        _sfxSlider.SetActive(false);
    }

    public void CloseQuit()
    {
        Time.timeScale = 1;
        _quitPanel.SetActive(false);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_index);
    }
}
