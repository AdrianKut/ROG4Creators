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
    private AudioSource audioSource;
    private void Awake()
    {
        yPos = transform.position.y;
        audioSource = GetComponent<AudioSource>();
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
        audioSource.Play();
        GameManager.gameManagerInstance.IncreaseMoney(moneyAmount);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(transform.GetChild(0).gameObject);
        Destroy(gameObject, 2f);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && PowerUpManager.ShieldActivated())
        {
            Die();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            Destroy(transform.GetChild(0).gameObject);
            Destroy(gameObject.GetComponent<Collider2D>());
            Destroy(gameObject,2f);
        }

    }
}