using System.Collections;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{

    private RectTransform transfom;
    private TextMeshProUGUI text;
    private AudioSource audioSource;
    void Start()
    {
        GameManager.gameManagerInstance.OnHighscore.AddListener(DisplayTextHighScore);

        text = this.gameObject.GetComponent<TextMeshProUGUI>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private IEnumerator Show()
    {
        audioSource.Play();
        transfom = this.gameObject.GetComponent<RectTransform>();
        transfom.localScale = new Vector3(0, 0, 0);

        LeanTween.rotateAround(gameObject, Vector3.forward, 360, 0.25f);
        this.gameObject.LeanScale(new Vector3(1, 1, 1), 1f).setEaseOutQuart();
        yield return new WaitForSeconds(1f);
        this.gameObject.LeanScale(new Vector3(0, 0, 0), 1f).setEaseInQuart();
        LeanTweenExt.LeanAlphaText(text, 0,1f).setEase(LeanTweenType.linear);
    }

    private void DisplayTextHighScore()
    {
        Debug.Log("New Highscore");
        StartCoroutine("Show");
    }
}
