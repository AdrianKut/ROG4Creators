using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BigTextOnMiddle : MonoBehaviour
{

    private TextMeshProUGUI text;
    private RectTransform transfom;
    private AudioSource audioSource;

    [Header("0 - HIGH SCORE | 1 - EXTRA MONEY")]
    [Space]
    [Header("Audio Clips")]
    public AudioClip[] clips;
    // 0 - New Highscore 
    // 1 - Money money money 

    public Dictionary<float, string> distanceToReachDictionary = new Dictionary<float, string>();

    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.GameManagerInstance;
        gameManager.OnHighscore.AddListener(DisplayTextHighScore);
        gameManager.OnDestroyMoneyPig.AddListener(DisplayTextExtraMoney);

        text = this.gameObject.GetComponent<TextMeshProUGUI>();
        audioSource = this.gameObject.GetComponent<AudioSource>();

        InitalizeDistanceToReach();
    }

    private void InitalizeDistanceToReach()
    {
        distanceToReachDictionary.Add(500, "GOOD!");
        distanceToReachDictionary.Add(1000, "WOW!");
        distanceToReachDictionary.Add(2500, "AMAZING!");
        distanceToReachDictionary.Add(5000, "OUTSTANDING!");
        distanceToReachDictionary.Add(10000, "AWESOME!");
        distanceToReachDictionary.Add(20000, "CHEATER!");
    }



    public void DisplayAfterReachDistance(float distance)
    {
        if (distanceToReachDictionary.ContainsKey(distance))
        {
            text.text = distanceToReachDictionary[distance];

            switch (distance)
            {
                case 500:
                    text.color = Color.yellow;
                    break;
                case 1000:
                    text.color = Color.red;
                    break;
                case 2500:
                    text.color = Color.green;
                    break;
                case 5000:
                    text.color = Color.blue;
                    break;
                case 10000:
                    text.color = Color.magenta;
                    break;
                case 20000:
                    text.color = Color.grey;
                    break;
            }

            distanceToReachDictionary.Remove(distance);

            StartCoroutine("Display");
            audioSource.PlayOneShot(clips[0]);
        }

    }


    private void DisplayTextHighScore()
    {
        text.text = "NEW HIGH SCORE!";
        text.color = Color.yellow;
        StartCoroutine("Display");
        audioSource.PlayOneShot(clips[0]);
    }

    private void DisplayTextExtraMoney()
    {
        int[] moneys = { 5, 5, 5, 10, 10, 15, 15, 20, 25, 50 };
        int randomMoneyAmount = Random.Range(0, moneys.Length);

        GameManager.GameManagerInstance.money += moneys[randomMoneyAmount];

        text.text = $"+{moneys[randomMoneyAmount]} $";
        text.color = Color.green;

        audioSource.PlayOneShot(clips[1]);
        StartCoroutine("Display");
    }


    private IEnumerator Display()
    {
        transfom = this.gameObject.GetComponent<RectTransform>();
        transfom.localScale = new Vector3(0, 0, 0);
        text.alpha = 1f;

        LeanTween.rotateAround(gameObject, Vector3.forward, 360, 0.25f);
        this.gameObject.LeanScale(new Vector3(1, 1, 1), 1f).setEaseOutQuart();
        yield return new WaitForSeconds(1f);
        this.gameObject.LeanScale(new Vector3(0, 0, 0), 1f).setEaseInQuart();
        LeanTweenExt.LeanAlphaText(text, 0, 1f).setEase(LeanTweenType.linear);
    }



}
