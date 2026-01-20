using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Feedback;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class Shoot : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private WeaponClass equippedWeapon;
    [SerializeField] private Projectile projectile;
    [SerializeField] private GunRecoil recoil;
    private AmmoParent ammoPlacement = null;
    [SerializeField] private Transform[] firePoints;
    private WeaponID id;
    private Magazine currentMagazine;

    [Header("Haptic Settings")]
    [SerializeField] private HapticImpulseData hapticData;
    private XRGrabInteractable controller;

    private bool isFiring;

    private float nextFireTime;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource emptyMagAudio;
    [SerializeField] private AudioSource[] shootAudio;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem shootParticles;
    private bool grabbed = false;
    private void Awake()
    {
        controller = GetComponent<XRGrabInteractable>();
        id = GetComponent<WeaponID>();
    }

    public void OnTriggerPressed()
    {
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
        if (HasNoAmmo())
        {
            emptyMagAudio.Play();
            return;
        }

        for (int i = 0; i < firePoints.Length; i++)
        {
            Projectile spawnedProjectile = Instantiate(projectile, firePoints[i].position, firePoints[i].rotation);
            spawnedProjectile.Fire(equippedWeapon, firePoints[i].forward);
        }

        currentMagazine.ConsumeBullet();
        UpdateAmmoVisuals();

        //Feedback
        recoil.Fire();
        shootParticles.Play();

        for (int i = 0; i < shootAudio.Length; i++)
        {
            shootAudio[i].Play();
        }

        SendHaptics();

        nextFireTime = Time.time + (1f / equippedWeapon.fireRate);
    }

    private IEnumerator BurstFire()
    {
        isFiring = true;

        for (int i = 0; i < equippedWeapon.burstCount; i++)
        {
            if (HasNoAmmo())
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

    void SendHaptics()
    {
        float ammoRatio = (float)GetAmmo() / equippedWeapon.magazineSize;

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

    public void OnMagazineInserted(SelectEnterEventArgs args)
    {
        currentMagazine = args.interactableObject.transform.GetComponent<Magazine>();
        UpdateAmmoVisuals();
    }

    public void OnMagazineRemoved(SelectExitEventArgs args)
    {
        currentMagazine = null;
        UpdateAmmoVisuals();
    }

    private int hands = 0;
    public void OnGrab(SelectEnterEventArgs args)
    {
        hands = Mathf.Min(hands + 1, 2);
        if (hands > 1) return;

        grabbed = true;

        XRBaseInteractor player = args.interactorObject as XRBaseInteractor; 
        BeltAmmo.Instance.SetAmmo(grabbed, id, player.gameObject.CompareTag("LeftHand"));

        OnGrabVisuals();
    }
    public void OnExitGrab(SelectExitEventArgs args)
    {
        hands = Mathf.Max(hands - 1, 0);
        if (hands > 0) return;

        grabbed = false; 
        XRBaseInteractor player = args.interactorObject as XRBaseInteractor;
        BeltAmmo.Instance.SetAmmo(grabbed, id, player.gameObject.CompareTag("LeftHand"));
        OnGrabVisuals();
    }
    private void OnGrabVisuals()
    {
        GameManager.Instance.GrabbedGun(grabbed);

        if (ammoPlacement != null)
        {
            ammoPlacement.SetColliders(grabbed);
            ammoPlacement.SetAmmo(GetAmmo(), equippedWeapon.magazineSize);
        }
    }

    public void UpdateAmmoVisuals()
    {
        if (ammoPlacement != null) ammoPlacement.SetAmmo(GetAmmo(), equippedWeapon.magazineSize);
    }

    private bool HasNoAmmo()
    {
        return currentMagazine == null || !currentMagazine.HasAmmo();
    }

    private int GetAmmo()
    {
        if (currentMagazine != null)
            return currentMagazine.CurrentAmmo;

        return 0;
    }
    public void AddAmmoVisuals(SelectEnterEventArgs m)
    {
        ammoPlacement = m.interactableObject.transform.gameObject.GetComponentInChildren<AmmoParent>();
        if (ammoPlacement) ammoPlacement.SetAmmo(GetAmmo(), equippedWeapon.magazineSize);
    }
    public void RemoveAmmoVisuals()
    {
        ammoPlacement = null;
    }
}
