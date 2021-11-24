using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.gameManagerInstance;
        gameManager.OnGameOver.AddListener(HidePowerUps);
    }

    private void HidePowerUps()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {

    }

    [Header("Shield")]
    public short shieldDuration = 3;
    public GameObject GameObjectShield;
    public int shieldCost = 10;
    public void BuyShield()
    {
        if (gameManager.money >= shieldCost)
        {
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


    public void BuySuperAmmo()
    {


    }


    public void BuyHighSpeed()
    {


    }

}
