using UnityEngine;

public enum EnemyType
{
    Monster,
    Obstacle,
    Platform
}

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

    [SerializeField]
    EnemyType enemyType;

    public EnemyType GetEnemyType() => enemyType;

    private void Awake()
    {
        yPos = transform.position.y;
        audioSource = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, 4, 0);
        transform.position = new Vector3(newPos.x, yPos, newPos.z);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die("bullet");
    }

    private void Die(string typeOfDeath)
    {
        switch (typeOfDeath)
        {

            case "shield":
            case "bullet":
                GameManager.gameManagerInstance.IncreaseMoney(moneyAmount);
                goto destroy;

            case "player":
                GameManager.gameManagerInstance.GameOver();
            destroy:
                audioSource.Play();
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(transform.GetChild(0).gameObject);
                Destroy(transform.GetComponent<SpriteRenderer>());
                ////Delete sprite 
                //if (transform.childCount >= 1)
                //    Destroy(transform.GetChild(0).gameObject);
                //else
                //    Destroy(transform.GetComponent<SpriteRenderer>());

                Destroy(gameObject.GetComponent<Collider2D>());
                Destroy(gameObject, 2f);
                break;

            case "obstacle":
                GameManager.gameManagerInstance.GameOver();
                break;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && PowerUpManager.ShieldActivated() && enemyType == EnemyType.Monster)
        {
            Die("shield");
        }
        else if (other.gameObject.CompareTag("Player") && PowerUpManager.ShieldActivated() && enemyType == EnemyType.Obstacle)
        {
            Die("shield");
        }
        else if (other.gameObject.CompareTag("Player") && enemyType == EnemyType.Monster)
        {
            Die("player");
        }
        else if (other.gameObject.CompareTag("Player") && enemyType == EnemyType.Obstacle)
        {
            Die("obstacle");
        }


    }
}