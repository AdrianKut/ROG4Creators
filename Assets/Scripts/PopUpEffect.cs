using System.Collections;
using UnityEngine;

public class PopUpEffect : MonoBehaviour
{
    public float effectSpeed = 3f;
    private RectTransform transfom;

    private void Start() => PopUp();
    private void OnEnable() => PopUp();

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PopUp()
    {
         transfom = this.gameObject.GetComponent<RectTransform>();
        transfom.localScale = new Vector3(0, 0, 0);
        this.gameObject.LeanScale(new Vector3(1, 1, 1), 1f).setEaseOutQuart();
    }

    private bool canRescale = true;
    public void ReScale()
    {
        if (canRescale)
            StartCoroutine(NewMethod());
    }

    private IEnumerator NewMethod()
    {
        canRescale = false;

        audioSource.Play();
        LeanTween.scale(this.gameObject, new Vector3(1.25f, 1.25f, 1.25f), 0.50f).setEasePunch();
        LeanTween.scale(this.gameObject, new Vector3(1, 1, 1), 0).setDelay(0.5f);

        yield return new WaitForSeconds(0.5f);
        canRescale = true;
    }


}