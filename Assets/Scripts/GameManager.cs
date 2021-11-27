using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum PowerUpType
{
    Shield = 10,
    HighSpeed = 12,
    SuperAmmo = 11,
    Laser = 15
}


public class GameManager : MonoBehaviour
{
    [Header("LEVEL UI")]
    public GameObject GameObjectLevelUI;

    [Header("Distance Score")]
    public float distanceCounter;
    public float distanceMultipier = 0.05f;
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

    public UnityEvent OnGameOverEvent;
    public float gameOverDelay = 1.5f;

    public static GameManager gameManagerInstance;
    private void Awake()
    {
        Application.targetFrameRate = 300;
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
        money += 0.002f;
        textMoney.SetText($"{money:F0} $");
    }



    private void UpdateTimerText()
    {
        distanceCounter += distanceMultipier;
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
        isPaused = true;
        OnPauseEvent?.Invoke();

        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        OnPauseEvent?.Invoke();
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
        isGameOver = true;
        OnGameOverEvent?.Invoke();
    }

    public void Retry() => SceneManager.LoadScene("Game Scene");
    public void QuitGame() => SceneManager.LoadScene("Menu");


    //TO DELETE
    #region Debug
    public void NewHighScoreOnClick()
    {
        OnHighscore?.Invoke();
    }

    public void AddMoney()
    {
        money += 10;
    }

    [SerializeField]
    GameObject monster;
    public void Spawn()
    {
        Instantiate(monster, new Vector3(3.47f, 2f, 0f), Quaternion.identity);
    }
    #endregion

}
