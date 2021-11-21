using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Distance Score")]
    public float distanceCounter;
    public TextMeshProUGUI textDistance;

    [Header("Best Distance Score")]
    public float bestDistanceCounter;
    public TextMeshProUGUI textBestDistance;

    void Start()
    {
        Time.timeScale = 1f;

        bestDistanceCounter = PlayerPrefs.GetFloat("bestScore", 0);
        UpdateBestScoreText();
    }

    void Update()
    {
        if (!isPaused && !isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.P))
                Pause();

            UpdateTimerText();
        }


    }

    private void UpdateTimerText()
    {
        distanceCounter += 0.05f;
        textDistance.SetText($"{distanceCounter:F0} M");
    }


    private void UpdateBestScoreText()
    {
        textBestDistance.SetText($"BEST: {bestDistanceCounter:F0} M");
    }


    #region Pause Menu
    [Header("Pause")]
    public GameObject PauseMenu;
    public bool isPaused = false;
    public UnityEvent OnPauseEvent;

    public void Pause()
    {
        OnPauseEvent?.Invoke();

        isPaused = true;
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        isPaused = false;
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
    #endregion


    [Header("Game Over")]
    public bool isGameOver = false;
    public GameObject GameOverObject;

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        GameOverObject.SetActive(true);
        PlayerPrefs.SetFloat("bestScore", distanceCounter);
    }

    public void Retry() => SceneManager.LoadScene("Game Scene");
    public void QuitGame() => SceneManager.LoadScene("Menu");

}
