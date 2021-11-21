using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform armTransform;
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private int damage = 40;

    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private float cameraShakeAmount = 0.05f;

    [SerializeField]
    private float cameraShakeLength = 0.1f;

    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private LayerMask collisionLayerMask;

    [SerializeField]
    private Vector2 maxArmRotations;

    [SerializeField]
    private AudioSource shootAudioSource;

    private float timer;

    private CameraShake cameraShake;

    private void Awake()
    {
        cameraShake = FindObjectOfType<CameraShake>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && timer <= 0)
        {
            timer = fireRate;
            StartCoroutine(Shoot());
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = pos - armTransform.position;
        dir.Normalize();
 
        if (transform.eulerAngles.y > 90)
        {
            dir.x *= -1;
        }
 
        float angle = Mathf.RoundToInt(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        angle = Mathf.Clamp(angle, maxArmRotations.x, maxArmRotations.y);
        armTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    private IEnumerator Shoot()
    {
        cameraShake.Shake(cameraShakeAmount, cameraShakeLength);
        shootAudioSource.Play();
        var hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, 9999, collisionLayerMask);
        GameObject impactGameObject = null;
        if (hitInfo)
        {
            var enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            impactGameObject = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;

        yield return new WaitForSeconds(0.75f);
        if (impactGameObject != null)
        {
            Destroy(impactGameObject);
        }
    }
}