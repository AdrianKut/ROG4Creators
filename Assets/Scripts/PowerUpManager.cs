using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PowerUpType
{
    Shield = 25,
    SlowMotion = 50,
    Laser = 75,
    SuperAmmo = 100,
    Nuke = 150
}

public class PowerUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject GameObjectParentPowerUpDurationIcons;

    [SerializeField]
    GameObject GameObjectChildPowerUpDurationIcons;

    [SerializeField]
    Sprite shieldSprite;

    [SerializeField]
    Sprite laserSprite;

    [SerializeField]
    Sprite superAmmoSprite;

    [SerializeField]
    Sprite highSpeedSprite;

    [SerializeField]
    Sprite nukeSprite;

    private LoopBackground loopBackground;
    private GameManager gameManager;
    private AudioSource audioSource;
    private GameObject player;

    public static PowerUpManager PowerUpManagerInstance;

    private void Awake()
    {
        if (PowerUpManagerInstance == null)
            PowerUpManagerInstance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");

        loopBackground = GameObject.FindGameObjectWithTag("Background").GetComponent<LoopBackground>();

        gameManager = GameManager.GameManagerInstance;
        gameManager.OnGameOverEvent.AddListener(HidePowerUpsUI);
        gameManager.OnPauseEvent.AddListener(HidePowerUpsUI);
    }

    private void Update()
    {
        ActivationPowerUpsByKeys();
    }

    private void ActivationPowerUpsByKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && buttonShield.interactable)
            BuyShield();

        if (Input.GetKeyDown(KeyCode.Alpha2) && buttonSlowMotion.interactable)
            BuySlowMotion();

        if (Input.GetKeyDown(KeyCode.Alpha3) && buttonLaser.interactable)
            BuyLaser();

        if (Input.GetKeyDown(KeyCode.Alpha4) && buttonSuperAmmo.interactable)
            BuySuperAmmo();

        if (Input.GetKeyDown(KeyCode.Alpha5) && buttonNuke.interactable)
            BuyNuke();
    }

    private void HidePowerUpsUI()
    {
        if (gameManager.isGameOver || gameManager.isPaused)
        {
            this.gameObject.SetActive(false);
        }
        else
            this.gameObject.SetActive(true);

    }

    private IEnumerator ChangeColorOfButtonToRed(Button thisButton)
    {
        thisButton.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        thisButton.GetComponent<Image>().color = Color.white;
    }

    private void ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, Sprite powerUpSprite, float powerUpDuration)
    {
        powerUpIcon = Instantiate(GameObjectChildPowerUpDurationIcons, GameObjectChildPowerUpDurationIcons.transform.position, Quaternion.identity);
        powerUpIcon.transform.SetParent(GameObjectParentPowerUpDurationIcons.transform);
        powerUpIcon.GetComponent<Image>().sprite = powerUpSprite;

        textPowerUpDuration = powerUpIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textPowerUpDuration.SetText("" + powerUpDuration);
    }

    #region Shield
    [Header("Shield")]
    public GameObject GameObjectShield;

    [SerializeField]
    private Button buttonShield;

    [SerializeField]
    private short shieldDuration;

    [SerializeField]
    private float timeToRenewShield;

    private static bool isShieldActivated = false;
    public static bool ShieldActivated() => isShieldActivated == true ? true : false;
    public void BuyShield()
    {
        if (gameManager.money >= (int)PowerUpType.Shield)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.Shield);
            StartCoroutine(EnableShield());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonShield));
    }

    private IEnumerator EnableShield()
    {
        isShieldActivated = true;
        var shield = Instantiate(GameObjectShield, GameObjectShield.transform.position, Quaternion.identity);

        buttonShield.interactable = false;

        LeanTween.rotateAround(shield, Vector3.forward, 360, 1.75f).setLoopClamp();

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, shieldSprite, shieldDuration);

        for (int i = shieldDuration; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            LeanTween.alpha(shield, 0.15f, 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.alpha(shield, 1.0f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.easeInQuad);
            yield return new WaitForSeconds(1f);
        }

        LeanTween.scale(shield, new Vector3(0,0,0f), 7f).setEasePunch().setLoopPingPong();
        LeanTween.alpha(shield, 0f, 0.8f).setEase(LeanTweenType.easeOutQuad);

        Destroy(shield, 1f);
        Destroy(powerUpIcon);

        isShieldActivated = false;
        yield return new WaitForSeconds(timeToRenewShield);
        buttonShield.interactable = true;
    }


    #endregion

    #region SuperAmmo
    [Header("SuperAmmo")]

    [SerializeField]
    private Button buttonSuperAmmo;

    [SerializeField]
    private short superAmmoDuration;

    [SerializeField]
    private float timeToRenewSuperAmmo;

    private static bool isSuperAmmoActivated = false;
    public static bool SuperAmmoActivated() => isSuperAmmoActivated == true ? true : false;
    public void BuySuperAmmo()
    {
        if (gameManager.money >= (int)PowerUpType.SuperAmmo)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.SuperAmmo);
            StartCoroutine(EnableSuperAmmo());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonSuperAmmo));
    }

    private IEnumerator EnableSuperAmmo()
    {
        isSuperAmmoActivated = true;
        var rayCastWeapon = player.GetComponent<RayCastWeapon>();

        buttonSuperAmmo.GetComponent<Button>().interactable = false;

        rayCastWeapon.ChangeValueOfFireRate(0.15f);

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, superAmmoSprite, superAmmoDuration);

        for (int i = superAmmoDuration; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        if (rayCastWeapon.GetNameOfCurrentWeapon() == "laser")
            rayCastWeapon.UseLaser();
        else if (rayCastWeapon.GetNameOfCurrentWeapon() == "rifle")
            rayCastWeapon.UseRifle();

        Destroy(powerUpIcon);

        isSuperAmmoActivated = false;
        yield return new WaitForSeconds(timeToRenewSuperAmmo);
        buttonSuperAmmo.interactable = true;

    }
    #endregion

    #region SlowMo

    [Header("Slow Motion")]
    public UnityEvent OnSlowMotionActivated;
    public UnityEvent OnSlowMotionDeactivated;

    [SerializeField]
    private Button buttonSlowMotion;

    [SerializeField]
    private float timeToRenewHighSpeed;

    public short slowMotionDuration = 10;
    private static bool isSlowMotionActivated = false;
    public static bool SlowMotionActivated() => isSlowMotionActivated == true ? true : false;
    public void BuySlowMotion()
    {
        if (gameManager.money >= (int)PowerUpType.SlowMotion)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.SlowMotion);
            StartCoroutine(EnableSlowMotion());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonSlowMotion));
    }

    private IEnumerator EnableSlowMotion()
    {
        isSlowMotionActivated = true;
        OnSlowMotionActivated?.Invoke();
        var currentSpeedBackground = loopBackground.speed;

        buttonSlowMotion.interactable = false;
        loopBackground.speed = (currentSpeedBackground / 2);

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, highSpeedSprite, slowMotionDuration);

        for (int i = slowMotionDuration; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        loopBackground.speed = currentSpeedBackground + 0.5f;

        isSlowMotionActivated = false;
        OnSlowMotionDeactivated?.Invoke();

        Destroy(powerUpIcon);
        yield return new WaitForSeconds(timeToRenewHighSpeed);
        buttonSlowMotion.interactable = true;
    }
    #endregion

    #region Laser
    [Header("Laser")]

    [SerializeField]
    private Button buttonLaser;

    [SerializeField]
    private short laserDuration;

    [SerializeField]
    private float timeToRenewLaser;

    private static bool isLaserActivated = false;
    public static bool LaserActivated() => isLaserActivated == true ? true : false;
    public void BuyLaser()
    {
        if (gameManager.money >= (int)PowerUpType.Laser)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.Laser);
            StartCoroutine(EnableLaser());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonLaser));
    }

    private IEnumerator EnableLaser()
    {
        buttonLaser.interactable = false;
        var rayCastWeapon = player.GetComponent<RayCastWeapon>();

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, laserSprite, laserDuration);

        if (isSuperAmmoActivated)
        {
            rayCastWeapon.UseLaser();
            rayCastWeapon.ChangeValueOfFireRate(0.15f);

            for (int i = laserDuration; i > 0; i--)
            {
                textPowerUpDuration.SetText("" + i);
                yield return new WaitForSeconds(1f);
            }

        }
        else
        {
            rayCastWeapon.UseLaser();
            for (int i = laserDuration; i > 0; i--)
            {
                textPowerUpDuration.SetText("" + i);
                yield return new WaitForSeconds(1f);
            }
        }

        Destroy(powerUpIcon);

        if (isSuperAmmoActivated)
        {
            rayCastWeapon.UseRifle();
            rayCastWeapon.ChangeValueOfFireRate(0.15f);
        }
        else
            rayCastWeapon.UseRifle();

        yield return new WaitForSeconds(timeToRenewLaser);
        buttonLaser.interactable = true;
    }
    #endregion

    #region Nuke
    [Header("Nuke")]

    [SerializeField]
    private Button buttonNuke;

    [SerializeField]
    private float nukeDuration;

    [SerializeField]
    private float timeToRenewNuke;

    [SerializeField]
    GameObject gameObjectNuke;

    public void BuyNuke()
    {
        if (gameManager.money >= (int)PowerUpType.Nuke)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.Nuke);
            StartCoroutine(EnableNuke());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonNuke));
    }

    private IEnumerator EnableNuke()
    {
        buttonNuke.interactable = false;

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, nukeSprite, 5);


        for (float i = nukeDuration; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        Instantiate(gameObjectNuke, gameObjectNuke.transform.position, Quaternion.identity);


        Destroy(powerUpIcon);

        yield return new WaitForSeconds(timeToRenewNuke);
        buttonNuke.interactable = true;
    }
    #endregion
}
