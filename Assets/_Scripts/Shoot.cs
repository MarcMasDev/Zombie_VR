using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponClass equippedWeapon;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform firePoint;

    [Header("Recoil Settings")]
    [SerializeField] private Transform gunTransform;
    [SerializeField] private float recoilAmount = 0.05f;
    [SerializeField] private float recoilRecoverySpeed = 5f;
    private Vector3 currentRecoil;
    private Vector3 recoilVelocity;


    [Header("Haptic Settings")]
    [SerializeField] private float hapticAmplitude = 0.3f;
    [SerializeField] private float hapticDuration = 0.1f;


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
    private void Update()
    {
        RecoverRecoil();
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
        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, transform.rotation);
        spawnedProjectile.Fire(equippedWeapon, firePoint.forward);

        currentAmmo--;
        ApplyAmmoVisuals();

        //Feedback
        ApplyRecoil();
        shootParticles.Play();
        shootAudio.Play();

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

            yield return new WaitForSeconds(delay);
        }
    }

    private void StopFiring()
    {
        isFiring = false;
    }

    private void RecoverRecoil()
    {
        currentRecoil = Vector3.SmoothDamp(currentRecoil, Vector3.zero, ref recoilVelocity, 1f / recoilRecoverySpeed);
        gunTransform.localRotation = Quaternion.Euler(currentRecoil);
    }

    private void ApplyRecoil()
    {
        //Randomized small recoil per shot
        float x = Random.Range(-0.5f, 0.5f);
        float y = recoilAmount;
        currentRecoil += new Vector3(-y, x, 0);
    }

    public void ApplyAmmoVisuals(bool dropped = false)
    {
        AmmoManager.Instance.UpdateAmmoManager(currentAmmo, equippedWeapon.magazineSize);

        if (dropped) AmmoManager.Instance.UpdateAmmoManager(0, 1);
    }
}
