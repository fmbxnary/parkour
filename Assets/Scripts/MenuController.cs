using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        // If the main menu (scene index 0) is loading the first level, reset nextLevelIndex
        if (sceneIndex == 1)
        {
            PlayerLife.nextLevelIndex = 1;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


