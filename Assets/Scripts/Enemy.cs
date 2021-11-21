using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float yOffset;
    
    [SerializeField]
    private GameObject deathEffect;
    
    private CharacterController2D player;
    private Vector3 targetPosition;
    private WaveManager waveManager;
    
    private void Awake()
    {
        player = FindObjectOfType<CharacterController2D>();
        waveManager = FindObjectOfType<WaveManager>();
    }
    
    private void FixedUpdate()
    {
        var pos = Vector3.MoveTowards (transform.localPosition, player.transform.position, Time.deltaTime * moveSpeed);
        targetPosition.x = pos.x;
        targetPosition.y = 2.94f - yOffset;
        transform.localPosition = targetPosition;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        waveManager.EnemyDead();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ( other.transform.GetComponent<CharacterController2D>() )
        {
            waveManager.PlayerDead();
        }
    }
}