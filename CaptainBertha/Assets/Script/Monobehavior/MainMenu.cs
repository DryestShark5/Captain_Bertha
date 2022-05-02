using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [System.Obsolete]
    public void PlayGame()
    {
        Application.LoadLevel(1);
    }
    [System.Obsolete]
    public void Menu()
    {
        Application.LoadLevel(0);
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}