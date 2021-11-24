using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private int moneyAmount;

    private float yPos;
    private void Awake()
    {
        yPos = transform.position.y;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, 4, 0);
        transform.position = new Vector3(newPos.x,yPos,newPos.z);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        GameManager.gameManagerInstance.IncreaseMoney(moneyAmount);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ( other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.gameManagerInstance.GameOver();
        }

        if (other.gameObject.CompareTag("Shield"))
        {
            Die();
        }
    }
}