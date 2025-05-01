using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private int _index;
    private void Start()
    {
        Next(_index);
    }

    public void Next(int index)
    {
        SceneManager.LoadScene(index);
    }
}
