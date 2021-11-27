using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    private GameManager gameManager;
    private AudioSource audioSource;
    private GameObject player;



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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");

        gameManager = GameManager.gameManagerInstance;
        gameManager.OnGameOverEvent.AddListener(HidePowerUpsUI);
        gameManager.OnPauseEvent.AddListener(HidePowerUpsUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && buttonShield.interactable)
            BuyShield();

        if (Input.GetKeyDown(KeyCode.Alpha2) && buttonHighSpeed.interactable)
            BuyHighSpeed();

        if (Input.GetKeyDown(KeyCode.Alpha3) && buttonLaser.interactable)
            BuyLaser();

        if (Input.GetKeyDown(KeyCode.Alpha4) && buttonSuperAmmo.interactable)
            BuySuperAmmo();
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

    private IEnumerator ChangeColorOfButtonToRed()
    {
        var thisButton = EventSystem.current.currentSelectedGameObject;
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

    [SerializeField]
    private Button buttonShield;

    [SerializeField]
    private short shieldDuration;

    [SerializeField]
    private float timeToRenewShield;

    private static bool isShieldActivated = false;
    public static bool ShieldActivated() => isShieldActivated == true ? true : false;
    public GameObject GameObjectShield;
    public void BuyShield()
    {
        if (gameManager.money >= (int)PowerUpType.Shield)
        {
            audioSource.Play();
            gameManager.BuyPowerUp(PowerUpType.Shield);
            StartCoroutine(EnableShield());
        }
        else
        {
            StartCoroutine(ChangeColorOfButtonToRed());
        }
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

        var a = LeanTween.alpha(this.gameObject, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad);

        Destroy(shield);
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
            gameManager.BuyPowerUp(PowerUpType.SuperAmmo);
            StartCoroutine(EnableSuperAmmo());
        }
        else
        {
            StartCoroutine(ChangeColorOfButtonToRed());
        }
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

    #region HighSpeed

    [Header("HighSpeed")]

    [SerializeField]
    private Button buttonHighSpeed;

    [SerializeField]
    private float timeToRenewHighSpeed;

    public short highSpeedDuration = 10;
    private static bool isHighSpeedActivated = false;
    public static bool HighSpeedActivated() => isHighSpeedActivated == true ? true : false;
    public void BuyHighSpeed()
    {
        if (gameManager.money >= (int)PowerUpType.HighSpeed)
        {
            audioSource.Play();
            gameManager.BuyPowerUp(PowerUpType.HighSpeed);
            StartCoroutine(EnableHighSpeed());
        }
        else
        {
            StartCoroutine(ChangeColorOfButtonToRed());
        }
    }

    private IEnumerator EnableHighSpeed()
    {
        var background = GameObject.FindGameObjectWithTag("Background");

        buttonHighSpeed.interactable = false;

        Time.timeScale = 2;
        background.GetComponent<LoopBackground>().speed *= 3;
        gameManager.distanceMultipier *= 2;

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, highSpeedSprite, highSpeedDuration);

        for (int i = highSpeedDuration; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }


        background.GetComponent<LoopBackground>().speed /= 3;
        gameManager.distanceMultipier /= 2;
        Time.timeScale = 1;

        Destroy(powerUpIcon);

        yield return new WaitForSeconds(timeToRenewHighSpeed);
        buttonHighSpeed.interactable = true;
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
            gameManager.BuyPowerUp(PowerUpType.Laser);
            StartCoroutine(EnableLaser());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed());
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

            rayCastWeapon.UseRifle();
        }
        else
        {
            rayCastWeapon.UseLaser();

            for (int i = laserDuration; i > 0; i--)
            {
                textPowerUpDuration.SetText("" + i);
                yield return new WaitForSeconds(1f);
            }

            rayCastWeapon.UseRifle();
        }

        Destroy(powerUpIcon);

        yield return new WaitForSeconds(timeToRenewLaser);
        buttonLaser.interactable = true;
    }
    #endregion
}
