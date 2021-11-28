using UnityEngine;

public class PopUpEffect : MonoBehaviour
{
    public float effectSpeed = 3f;
    private RectTransform transfom;

    private void Start() => PopUp();
    private void OnEnable() => PopUp();

    private void PopUp()
    {
        transfom = this.gameObject.GetComponent<RectTransform>();
        transfom.localScale = new Vector3(0, 0, 0);
        this.gameObject.LeanScale(new Vector3(1, 1, 1), 1f).setEaseOutQuart();

    }
}