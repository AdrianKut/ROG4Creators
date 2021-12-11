using UnityEngine;

public class BounceAndMoveLeft : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        LeanTween.moveY(this.gameObject, 6f, 0.8f).setEaseInOutQuad().setLoopPingPong();
    }

    void Update()
    {
        Vector3 newPos = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, 4, 0);
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);

        if (transform.position.x <= -20f)
            Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.name.Contains("MysteriousBox"))
        {
            GameManager.GameManagerInstance.OnDestroyMysteriousBox?.Invoke();
        }
        else if (this.gameObject.name.Contains("MoneyBank"))
        {
            GameManager.GameManagerInstance.OnDestroyMoneyPig?.Invoke();
        }
    }

}
