using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum PowerUpType
{
    Shield = 10,
    SuperAmmo = 11,
    HighSpeed = 12


}


public class GameManager : MonoBehaviour
{
    [Header("LEVEL UI")]
    public GameObject GameObjectLevelUI;

    [Header("Distance Score")]
    public float distanceCounter;
    public TextMeshProUGUI textDistance;

    [Header("")]
    public float money;
    public TextMeshProUGUI textMoney;

    [Header("Best Distance Score")]
    public float currentBestScore;
    public bool isNewHighScore = false;
    public TextMeshProUGUI textBestDistance;
    public UnityEvent OnHighscore;

    [Header("Game Over")]
    public bool isGameOver = false;

    public void BuyPowerUp(PowerUpType powerUpType)
    {

        switch (powerUpType)
        {
            case PowerUpType.Shield:
                money -= (int)powerUpType;
                break;

            case PowerUpType.SuperAmmo:
                money -= (int)powerUpType;
                break;

            case PowerUpType.HighSpeed:
                money -= (int)powerUpType;
                break;
        }
    }

    public GameObject GameOverObject;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI NewHighScoreText;

    public void IncreaseMoney(int moneyAmount) => money += moneyAmount;

    public UnityEvent OnGameOver;
    public float gameOverDelay = 1.5f;

    public static GameManager gameManagerInstance;
    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
        }
    }


    void Start()
    {
        AudioListener.volume = 1f;
        AudioListener.pause = false;

        Time.timeScale = 1f;
        currentBestScore = PlayerPrefs.GetFloat("bestScore");
        UpdateBestScoreText();
    }

    void Update()
    {
        if (!isPaused && !isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.P))
                Pause();

            UpdateTimerText();
            IncreaseMoneyPerSeconds();
        }

        if (isGameOver)
        {
            gameOverDelay -= Time.deltaTime * 1f;
            FinalScoreText.SetText("YOUR SCORE: \n " + textDistance.text);

            if (gameOverDelay <= 0)
            {
                GameObjectLevelUI.SetActive(false);
                Time.timeScale = 0f;
                GameOverObject.SetActive(true);

                //Save better distance score    
                if (isNewHighScore)
                {
                    PlayerPrefs.SetFloat("bestScore", distanceCounter);
                    NewHighScoreText.SetText("NEW HIGH SCORE!");
                }

                //Mute all sounds
                AudioListener.pause = true;
                AudioListener.volume = 0;


            }
        }
    }

    private void IncreaseMoneyPerSeconds()
    {
        money += 0.001f;
        textMoney.SetText($"{money:F0} $");
    }

    private void UpdateTimerText()
    {
        distanceCounter += 0.05f;
        textDistance.SetText($"{distanceCounter:F0} M");

        if ((currentBestScore < distanceCounter) && !isNewHighScore)
        {
            isNewHighScore = true;
            OnHighscore?.Invoke();
        }
    }


    private void UpdateBestScoreText()
    {
        textBestDistance.SetText($"BEST: {currentBestScore:F0} M");
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

    public void GameOver()
    {
        OnGameOver?.Invoke();
        isGameOver = true;
    }

    public void Retry() => SceneManager.LoadScene("Game Scene");
    public void QuitGame() => SceneManager.LoadScene("Menu");


    //TO DELETE
    #region Debug
    public void NewHighScoreOnClick()
    {
        OnHighscore?.Invoke();
    }
    #endregion

}
