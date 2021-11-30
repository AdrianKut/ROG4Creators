using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBank : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        LeanTween.moveY(this.gameObject, 5f, 1f).setEaseInOutCubic().setLoopPingPong();    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
}
