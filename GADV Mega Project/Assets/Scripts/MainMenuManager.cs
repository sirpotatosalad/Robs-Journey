using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // these allow functionality of menu buttons in the main menu

    // starting the game by loading the next scene in the buildIndex
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    // quits the application
    public void QuitGame()
    {
        Debug.Log("Quitting Game!");
        Application.Quit();
    }
}
