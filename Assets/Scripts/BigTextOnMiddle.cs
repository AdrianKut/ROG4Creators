using System.Collections;
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
    void Start()
    {
        GameManager.GameManagerInstance.OnHighscore.AddListener(DisplayTextHighScore);
        GameManager.GameManagerInstance.OnDestroyMoneyPig.AddListener(DisplayTextExtraMoney);

        text = this.gameObject.GetComponent<TextMeshProUGUI>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
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
