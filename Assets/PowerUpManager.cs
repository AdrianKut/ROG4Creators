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

    void Update()
    {

    }

    [Header("Shield")]
    public short shieldDuration = 3;
    public GameObject GameObjectShield;

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

        yield return new WaitForSeconds(1f);
        thisButton.GetComponent<Button>().interactable = true;
    }

    [Header("SuperAmmo")]
    public short superAmmoDuration = 10;
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
        var thisButton = EventSystem.current.currentSelectedGameObject;

        thisButton.GetComponent<Button>().interactable = false;

        player.GetComponent<RayCastWeapon>().ChangeValueOfFireRate(0.15f);
        yield return new WaitForSeconds(superAmmoDuration);
        player.GetComponent<RayCastWeapon>().ChangeValueOfFireRate(1f);

        yield return new WaitForSeconds(5f);
        thisButton.GetComponent<Button>().interactable = true;
    }

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


    [Header("Laser")]
    public short laserDuration = 5;
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

        thisButton.GetComponent<Button>().interactable = false;

        player.GetComponent<RayCastWeapon>().UseLaser();
        yield return new WaitForSeconds(laserDuration);
        player.GetComponent<RayCastWeapon>().UseRifle();

        yield return new WaitForSeconds(5f);
        thisButton.GetComponent<Button>().interactable = true;
    }
}
