using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private WeaponClass weapon;

    [Header("Blood VFX")]
    [SerializeField] private GameObject[] bloodVFXs;

    [Header("Impact VFX")]
    [SerializeField] private GameObject[] impactVFXs;

    public void Fire(WeaponClass weapon, Vector3 dir)
    {
        this.weapon = weapon;

        if (rb == null) rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * weapon.projectileSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Weapon")) return;

        IDamageable damageable = collision.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            if (!collision.collider.TryGetComponent<Hitbox>(out var h)) return;

            HitboxType hitbox = h.hitboxType;
            damageable.TakeDamage(GetDamage(hitbox), hitbox);
            ApplyBloodVFX(collision.contacts[0].normal, collision.transform);
        }
        else
        {
            ApplyGenericVFX(collision.contacts[0].normal, collision.transform);
        }

        Destroy(gameObject);
    }
    private float GetDamage(HitboxType hitbox)
    {
        float finalDamage = weapon.damage;

        if (hitbox == HitboxType.Head)
        {
            finalDamage *= weapon.headshotMultiplier;
        }
        else if (hitbox == HitboxType.Limb)
        {
            finalDamage *= weapon.limbMultiplier;
        }

        return finalDamage;
    }

    private void ApplyBloodVFX(Vector3 rot, Transform parent)
    {
        for (int i = 0; i < bloodVFXs.Length; i++)
        {
            Instantiate(bloodVFXs[i], transform.position, Quaternion.LookRotation(rot))
                .transform.SetParent(parent);
        }
    }
    private void ApplyGenericVFX(Vector3 rot, Transform parent)
    {
        for (int i = 0; i < impactVFXs.Length; i++)
        {
            Instantiate(impactVFXs[i], transform.position, Quaternion.LookRotation(rot))
                .transform.SetParent(parent);
        }
    }
}
