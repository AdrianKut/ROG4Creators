using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    private GameManager gameManager;
    private AudioSource audioSource;
    private GameObject player;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");

        gameManager = GameManager.gameManagerInstance;
        gameManager.OnGameOverEvent.AddListener(HidePowerUps);
        gameManager.OnPauseEvent.AddListener(HidePowerUps);

    }

    private void HidePowerUps()
    {
        this.gameObject.SetActive(false);
    }

    #region Shield
    [Header("Shield")]
    [SerializeField]
    private short shieldDuration = 3;
    private static bool isShieldActivated = false;
    public GameObject GameObjectShield;
    public static bool ShieldActivated() => isShieldActivated == true ? true : false;
    public void BuyShield()
    {
        if (gameManager.money >= (int)PowerUpType.Shield)
        {
            audioSource.Play();
            gameManager.BuyPowerUp(PowerUpType.Shield);
            StartCoroutine(EnableShield());
        }
    }

    private IEnumerator EnableShield()
    {
        isShieldActivated = true;
        var shield = Instantiate(GameObjectShield, GameObjectShield.transform.position, Quaternion.identity);

        var thisButton = EventSystem.current.currentSelectedGameObject;
        thisButton.GetComponent<Button>().interactable = false;

        LeanTween.rotateAround(shield, Vector3.forward, 360, 1.75f).setLoopClamp();

        for (int i = 0; i < shieldDuration; i++)
        {
            LeanTween.alpha(shield, 0.15f, 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.alpha(shield, 1.0f, 0.5f).setDelay(0.5f).setEase(LeanTweenType.easeInQuad);
            yield return new WaitForSeconds(1.5f);
        }

        var a = LeanTween.alpha(this.gameObject, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad);
        Destroy(shield);

        isShieldActivated = false;
        yield return new WaitForSeconds(1f);
        thisButton.GetComponent<Button>().interactable = true;
    }
    #endregion

    #region SuperAmmo
    [Header("SuperAmmo")]
    [SerializeField]
    private short superAmmoDuration = 10;
    private bool isSuperAmmoActivated = false;
    public void BuySuperAmmo()
    {
        if (gameManager.money >= (int)PowerUpType.SuperAmmo)
        {
            audioSource.Play();
            gameManager.BuyPowerUp(PowerUpType.SuperAmmo);
            StartCoroutine(EnableSuperAmmo());
        }
    }

    private IEnumerator EnableSuperAmmo()
    {
        isSuperAmmoActivated = true;
        var thisButton = EventSystem.current.currentSelectedGameObject;
        var rayCastWeapon = player.GetComponent<RayCastWeapon>();
        thisButton.GetComponent<Button>().interactable = false;

        rayCastWeapon.ChangeValueOfFireRate(0.15f);
        yield return new WaitForSeconds(superAmmoDuration);

        if (rayCastWeapon.GetNameOfCurrentWeapon() == "laser")
            rayCastWeapon.UseLaser();
        else if (rayCastWeapon.GetNameOfCurrentWeapon() == "rifle")
            rayCastWeapon.UseRifle();

        isSuperAmmoActivated = false;
        yield return new WaitForSeconds(5f);
        thisButton.GetComponent<Button>().interactable = true;
    }
    #endregion

    #region HighSpeed
    [Header("HighSpeed")]
    public short highSpeedDuration = 5;
    public void BuyHighSpeed()
    {
        if (gameManager.money >= (int)PowerUpType.HighSpeed)
        {
            audioSource.Play();
            gameManager.BuyPowerUp(PowerUpType.HighSpeed);
            StartCoroutine(EnableHighSpeed());
        }
    }

    private IEnumerator EnableHighSpeed()
    {
        var thisButton = EventSystem.current.currentSelectedGameObject;
        var background = GameObject.FindGameObjectWithTag("Background");

        thisButton.GetComponent<Button>().interactable = false;

        background.GetComponent<LoopBackground>().speed *= 2;
        gameManager.distanceMultipier *= 2;
        yield return new WaitForSeconds(highSpeedDuration);
        background.GetComponent<LoopBackground>().speed /= 2;
        gameManager.distanceMultipier /= 2;

        yield return new WaitForSeconds(5f);
        thisButton.GetComponent<Button>().interactable = true;
    }
    #endregion

    #region LASER
    [Header("Laser")]
    [SerializeField]
    private short laserDuration = 5;
    public void BuyLaser()
    {
        if (gameManager.money >= (int)PowerUpType.Laser)
        {
            audioSource.Play();
            gameManager.BuyPowerUp(PowerUpType.Laser);
            StartCoroutine(EnableLaser());
        }
    }

    private IEnumerator EnableLaser()
    {
        var thisButton = EventSystem.current.currentSelectedGameObject;
        var rayCastWeapon = player.GetComponent<RayCastWeapon>();

        thisButton.GetComponent<Button>().interactable = false;

        if (isSuperAmmoActivated)
        {
            rayCastWeapon.UseLaser();
            rayCastWeapon.ChangeValueOfFireRate(0.15f);
            yield return new WaitForSeconds(laserDuration);
            rayCastWeapon.UseRifle();
        }
        else
        {
            rayCastWeapon.UseLaser();
            yield return new WaitForSeconds(laserDuration);
            rayCastWeapon.UseRifle();
        }

        yield return new WaitForSeconds(5f);
        thisButton.GetComponent<Button>().interactable = true;
    }
    #endregion
}
