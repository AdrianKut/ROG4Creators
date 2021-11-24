using UnityEngine;

public class PrefabWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private GameObject bulletPrefab;


    [SerializeField]
    private AudioSource shootAudioSource;

    [SerializeField]
    private float fireRate = 0.1f;

    private float timer;
    private void Update()
    {
        if (Input.GetButton("Fire1") && timer <= 0)
        {
            timer = fireRate;
            Shoot();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        shootAudioSource.Play();
    }
}