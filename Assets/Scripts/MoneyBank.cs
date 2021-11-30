using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyBank : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        LeanTween.moveY(this.gameObject, 6f, 0.8f).setEaseInOutQuad().setLoopPingPong();    
    }

    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, 4, 0);
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);

        if (newPos.x <= -20f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.GameManagerInstance.OnDestroyMoneyPig?.Invoke();
    }

}
