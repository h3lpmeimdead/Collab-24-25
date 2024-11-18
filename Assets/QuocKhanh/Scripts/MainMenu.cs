using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject levelSelect;

  public void OpenLevelSelect()
  {
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);
  }

  public void CloseLevelSelect()
  {
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
  }
  public void Quit()
  {
        Application.Quit();  
  }
  public void OpenCredits()
  {
        SceneManager.LoadScene("Credits");
  }
  public void ToLevel1()
    {
        SceneManager.LoadScene("TestLevel");
    }
    public void ToLevel2()
    {
        SceneManager.LoadScene("TestLevel2");
    }
  
}
