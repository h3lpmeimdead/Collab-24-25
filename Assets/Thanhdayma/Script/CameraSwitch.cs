using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _grappleCam;
    [SerializeField] private GameObject _cannonCam;

    private void Start()
    {
        _grappleCam.SetActive(false);
        _cannonCam.SetActive(true);        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_grappleCam != null && _cannonCam == null)
            {
                _grappleCam.SetActive(false);
                _cannonCam.SetActive(true);
            }
            else
            {
                _grappleCam.SetActive(true);
                _cannonCam.SetActive(false);
            }
        }
    }
}
