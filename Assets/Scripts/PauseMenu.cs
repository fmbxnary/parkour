using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    public bool isPaused = false;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelButtonContainer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f; // pauses the game
        pauseMenuUI.SetActive(true);

        // Remove old level buttons
        foreach (Transform child in levelButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new level buttons
        for (int i = 1; i <= PlayerLife.nextLevelIndex; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, levelButtonContainer);
            levelButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Level " + i;
            int levelIndex = i; // Capture index in local variable for closure
            levelButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    public void LoadLevel(int levelIndex)
    {
        Time.timeScale = 1f; // unpauses the game
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene(levelIndex);
    }



    public void PauseGame()
    {
        Time.timeScale = 0f; // pauses the game
        pauseMenuUI.SetActive(true);

        // Remove old level buttons
        foreach (Transform child in levelButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new level buttons
        for (int i = 1; i <= PlayerLife.nextLevelIndex; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, levelButtonContainer);
            levelButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Level " + i;
            int levelIndex = i; // Capture index in local variable for closure
            levelButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));
        }
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
