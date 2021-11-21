using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private int damage = 40;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject impactEffect;

    private Collider2D collider2D;
    
    private void Start()
    {
        rb.velocity = transform.right * speed;
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        var enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        
        StartCoroutine(SpawnImpactEffect());
    }

    private IEnumerator SpawnImpactEffect()
    {
        var impactGameObject = Instantiate(impactEffect, transform.position, transform.rotation);

        yield return new WaitForSeconds(1f);
        if (impactGameObject != null)
        {
            Destroy(impactGameObject);
        }

        Destroy(gameObject);
    }
}