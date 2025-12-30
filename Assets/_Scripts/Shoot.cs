using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Feedback;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Shoot : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponClass equippedWeapon;
    [SerializeField] private Projectile projectile;
    [SerializeField] private GunRecoil recoil;
    [SerializeField] private Transform[] firePoints;


    [Header("Haptic Settings")]
    [SerializeField] private HapticImpulseData hapticData;
    private XRGrabInteractable controller;

    private int currentAmmo;
    private bool isReloading;
    private bool isFiring;

    private float nextFireTime;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource emptyMagAudio;
    [SerializeField] private AudioSource shootAudio;
    [SerializeField] private AudioSource reloadAudio;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem shootParticles;
    private bool grabbed = false;
    private void Awake()
    {
        mainCamera = Camera.main;

        currentAmmo = equippedWeapon.magazineSize;
        controller = GetComponent<XRGrabInteractable>();
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

        for (int i = 0; i < firePoints.Length; i++)
        {
            Projectile spawnedProjectile = Instantiate(projectile, firePoints[i].position, firePoints[i].rotation);
            spawnedProjectile.Fire(equippedWeapon, firePoints[i].forward);
        }

        currentAmmo--;
        ApplyAmmoVisuals();

        //Feedback
        recoil.Fire();
        shootParticles.Play();
        shootAudio.Play();
        SendHaptics();

        nextFireTime = Time.time + (1f / equippedWeapon.fireRate);
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

    public void ApplyAmmoVisuals(bool dropped = false)
    {
        grabbed = !dropped;
        AmmoManager.Instance.UpdateAmmoManager(currentAmmo, equippedWeapon.magazineSize);

        if (dropped) AmmoManager.Instance.UpdateAmmoManager(0, 1);
    }

    void SendHaptics()
    {
        float ammoRatio = (float)currentAmmo / equippedWeapon.magazineSize;

        foreach (var interactor in controller.interactorsSelecting)
        {
            var feedback = interactor.transform.GetComponentInChildren<SimpleHapticFeedback>();
            HapticImpulseData finalHapticData = new HapticImpulseData();
            finalHapticData.duration = hapticData.duration;
            finalHapticData.amplitude = hapticData.amplitude * ammoRatio;
            finalHapticData.frequency = hapticData.frequency;

            feedback?.SendHapticImpulse(finalHapticData);
        }
    }

    [SerializeField] private InputActionReference reloadAction;

    private void OnEnable()
    {
        reloadAction.action.Enable();
    }

    private void OnDisable()
    {
        reloadAction.action.Disable();
    }

    private void Update()
    {
        if (grabbed && !isReloading && currentAmmo < equippedWeapon.magazineSize)
        {
            if (reloadAction.action.WasPressedThisFrame() || currentAmmo <= 0) Reload();
        }
    }

    public void Reload()
    {
        if (isReloading)
            return;

        StartCoroutine(ReloadAsync());
    }
    private IEnumerator ReloadAsync()
    {
        isReloading = true;
        reloadAudio.Play();

        yield return new WaitForSeconds(equippedWeapon.reloadTime);

        currentAmmo = equippedWeapon.magazineSize;
        ApplyAmmoVisuals(false);
        isReloading = false;
    }
}
