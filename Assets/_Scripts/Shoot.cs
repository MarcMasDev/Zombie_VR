using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponClass equippedWeapon;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform firePoint;


    private int currentAmmo;
    private bool isReloading;
    private bool isFiring;

    private float nextFireTime;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource emptyMagAudio;
    [SerializeField] private AudioSource shootAudio;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem shootParticles;

    private void Awake()
    {
        mainCamera = Camera.main;

        currentAmmo = equippedWeapon.magazineSize;
    }

    public void OnTriggerPressed()
    {
        if (isReloading)
            return;

        switch (equippedWeapon.fireMode)
        {
            case FireMode.SemiAuto:
                ShootWeapon();
                break;

            case FireMode.FullAuto:
                StartFullAuto();
                break;

            case FireMode.Burst:
                StartBurst();
                break;
        }
    }

    public void OnTriggerReleased()
    {
        StopFiring();
    }

    private void StartFullAuto()
    {
        if (isFiring)
            return;

        StartCoroutine(FullAutoRoutine());
    }
    private void StartBurst()
    {
        if (isFiring)
            return;

        StartCoroutine(BurstFire());
    }
    private void ShootWeapon()
    {
        if (Time.time < nextFireTime) return;
        if (currentAmmo <= 0)
        {
            emptyMagAudio.Play();
            return;
        }

        shootParticles.Play();
        shootAudio.Play();
        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, transform.rotation);
        spawnedProjectile.Fire(equippedWeapon, firePoint.forward);

        currentAmmo--;
        nextFireTime = Time.time + (1f / equippedWeapon.fireRate);
    }

    public void Reload()
    {
        if (isReloading)
            return;

        StartCoroutine(ReloadAsync());
    }

    private IEnumerator ReloadAsync()
    {
        if (isReloading || currentAmmo == equippedWeapon.magazineSize)
            yield break;

        isReloading = true;

        yield return new WaitForSeconds(equippedWeapon.reloadTime);

        currentAmmo = equippedWeapon.magazineSize;
        isReloading = false;
    }

    private IEnumerator BurstFire()
    {
        isFiring = true;

        for (int i = 0; i < equippedWeapon.burstCount; i++)
        {
            if (currentAmmo <= 0)
                break;

            ShootWeapon();
            currentAmmo--;

            yield return new WaitForSeconds(1f / equippedWeapon.fireRate);
        }

        isFiring = false;
    }

    private IEnumerator FullAutoRoutine()
    {
        isFiring = true;

        float delay = 1f / equippedWeapon.fireRate;

        while (isFiring)
        {
            ShootWeapon();
            currentAmmo--;

            yield return new WaitForSeconds(delay);
        }
    }

    private void StopFiring()
    {
        isFiring = false;
    }
}
