using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private float currentAmmo = 2f;
    [SerializeField] private float fillSpeed = 1.25f;
    [SerializeField] private float maxAmmo = 2f;
    [SerializeField] private AudioSource sound;
    [SerializeField] private ParticleSystem particle;

    private bool shooting = false;
    private bool grabbed = false;
    public void SetShoot(bool shoot)
    {
        if (shoot)
        {
            sound.Play();
            particle.Play();
        }
        else if (!shoot)
        {
            sound.Stop();
            particle.Stop();
        }

        shooting = shoot;
    }
    public void SetGrabbed(bool grab)
    {
        grabbed = grab;

        if (!grab)
        {
            AmmoManager.Instance.UpdateAmmoManager(0, 1);
            SetShoot(false);
        }
        else AmmoManager.Instance.UpdateAmmoManagerFloat(currentAmmo, maxAmmo);
    }
    private void Update()
    {
        if (grabbed)
        {
            if (shooting) currentAmmo -= Time.deltaTime*fillSpeed;
            if (currentAmmo <= 0) SetShoot(false);

            AmmoManager.Instance.UpdateAmmoManagerFloat(currentAmmo, maxAmmo);
        }
        
        if (!shooting)
        {
            if (currentAmmo <= maxAmmo) currentAmmo += Time.deltaTime;
            if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable == null) return;
        if (!shooting) return;

        damageable.TakeDamage(damage, HitboxType.Fire);
    }
}
