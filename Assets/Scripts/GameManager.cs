using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



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

    public GameObject GameOverObject;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI NewHighScoreText;

    public void IncreaseMoney(int moneyAmount) => money += moneyAmount;
    public void BuyPowerUpTypeAndDecreaseMoney(PowerUpType powerUpType) => money -= (int)powerUpType;


    public UnityEvent OnGameOverEvent;
    public float gameOverDelay = 1.5f;

    public static GameManager GameManagerInstance;
    private void Awake()
    {
        Application.targetFrameRate = 300;

        if (GameManagerInstance == null)
            GameManagerInstance = this;
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
        money += 0.005f;
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
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("Menu");
    }
    #endregion

    public void GameOver()
    {
        isGameOver = true;
        OnGameOverEvent?.Invoke();
    }

    public void Retry() => SceneManager.LoadScene("Game Scene");

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

    private bool xd = false;
    public void IncreaseSpeed()
    {
        xd = !xd;
        if (!xd)
        {
            distanceMultipier *= 10;
            Time.timeScale *= 10f;
        }
        else if (xd)
        {
            distanceMultipier = 0.05f;
            Time.timeScale = 1f;
        }

    }
    #endregion

}
