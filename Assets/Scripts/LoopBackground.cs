using System;
using System.Collections;
using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    
    [SerializeField]
    public float xBound = -47.32f;

    [SerializeField]
    public float speed = 5f;

    [SerializeField]
    private  Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        speed = 5f;

        StartCoroutine(IncreaseSpeedBackground());
    }

    private void LateUpdate()
    {
        if (GameManager.GameManagerInstance.isGameOver == false)
        {
            Vector3 newPos = new Vector3(transform.position.x - speed * Time.deltaTime, 4, 0);
            transform.position = newPos;

            if (transform.position.x <= xBound)
                transform.position = startPos;
        }
    }

    float increaseSpeedDelay = 10f;
    IEnumerator IncreaseSpeedBackground()
    {
        while (true)
        {       
            yield return new WaitForSeconds(increaseSpeedDelay);
            speed += 0.5f;
        }
    }

   
}

