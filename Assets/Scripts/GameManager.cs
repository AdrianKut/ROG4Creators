using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [Header("LEVEL UI")]
    public GameObject GameObjectLevelUI;
    public GameObject AskToExitGameObject;
    public GameObject GameObjectPowerUpUI;

    [Header("Player")]
    public GameObject PlayerGameObject;
    private Vector3 startPosPlayer;

    [Header("Distance Score")]
    public float distanceCounter;
    public float distanceMultipier = 0.3f;
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
    private BigTextOnMiddle bigTextOnMiddle;

    public void IncreaseMoney(int moneyAmount) => money += moneyAmount;
    public void BuyPowerUpTypeAndDecreaseMoney(PowerUpType powerUpType) => money -= (int)powerUpType;

    public UnityEvent OnGameOverEvent;
    public float gameOverDelay = 1.5f;

    private AudioSource audioSource;

    public UnityEvent OnDestroyMoneyPig;

    public static GameManager GameManagerInstance;
    private void Awake()
    {
        if (GameManagerInstance == null)
            GameManagerInstance = this;

        audioSource = GetComponent<AudioSource>();
        startPosPlayer = PlayerGameObject.transform.position;

    }


    [SerializeField]
    TextMeshProUGUI textPressAnyKeyToStart;
    private bool isStarted;
    void Start()
    {
        HideUI();
        Time.timeScale = 0f;
        isStarted = false;

        currentBestScore = PlayerPrefs.GetFloat("bestScore");
        UpdateBestScoreText();

        bigTextOnMiddle = GameObject.Find("Big Text On Middle").GetComponent<BigTextOnMiddle>();
    }


    private void Update()
    {
        if (Input.anyKey && isStarted == false)
        {
            isStarted = true;
            Time.timeScale = 1f;
            AudioListener.volume = 1f;
            AudioListener.pause = false;
            audioSource.Play();

            StartCoroutine("HideTextPressAnyKeyToStart");
            ShowUI();
        }

        if (!isPaused && !isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                Pause();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                Resume();
        }
    }

    void FixedUpdate()
    {
        if (!isPaused && !isGameOver && isStarted)
        {
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
                GameOverObject.SetActive(true);

                //Save better distance score    
                if (isNewHighScore)
                {
                    PlayerPrefs.SetFloat("bestScore", distanceCounter);
                    NewHighScoreText.SetText("NEW HIGH SCORE!");
                }

                audioSource.Stop();
            }
        }

        //Display Big text with reached distance
        bigTextOnMiddle.DisplayAfterReachDistance((int)distanceCounter);
    }


    private IEnumerator HideTextPressAnyKeyToStart()
    {
        var gameObject = textPressAnyKeyToStart.gameObject;
        textPressAnyKeyToStart.alpha = 1f;

        LeanTweenExt.LeanAlphaText(textPressAnyKeyToStart, 0, 1.25f).setEaseOutSine();

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


    private void HideUI()
    {
        GameObjectLevelUI.SetActive(false);
        PlayerGameObject.SetActive(false);
        GameObjectPowerUpUI.SetActive(false);
    }

    private void ShowUI()
    {
        GameObjectLevelUI.SetActive(true);
        PlayerGameObject.SetActive(true);
        GameObjectPowerUpUI.SetActive(true);
    }


    private void IncreaseMoneyPerSeconds()
    {
        money += 0.02f;
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


    #region Buttons
    [Header("Pause")]
    public GameObject PauseGameObject;
    public bool isPaused = false;
    public void Pause()
    {
        audioSource.Pause();
        PlayerGameObject.SetActive(false);

        PlayerGameObject.transform.GetChild(1).GetComponent<LineRenderer>().enabled = false;
        isPaused = true;

        PauseGameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        audioSource.Play();
        AskToExitGameObject.SetActive(false);

        PlayerGameObject.transform.position = startPosPlayer;
        PlayerGameObject.SetActive(true);

        isPaused = false;

        PauseGameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        PauseGameObject.SetActive(false);
        AskToExitGameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void Retry() => SceneManager.LoadScene("Game Scene");
    #endregion

    public void GameOver()
    {
        isGameOver = true;
        OnGameOverEvent?.Invoke();
    }


    //TO DELETE
    #region Debug
    public void NewHighScoreOnClick()
    {
        OnHighscore?.Invoke();
    }

    public void AddMoney()
    {
        money += 50;
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
        if (!xd)
        {
            Time.timeScale *= 20f;
        }
        else if (xd)
        {
            Time.timeScale = 1f;
        }

        xd = !xd;

    }
    #endregion
}
