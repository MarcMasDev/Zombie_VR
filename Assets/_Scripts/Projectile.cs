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
        IDamageable damageable = collision.collider.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(weapon.damage);

            for (int i = 0; i < bloodVFXs.Length; i++)
            {
                Instantiate(bloodVFXs[i], transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
            }
        }
        else
        {
            for (int i = 0; i < impactVFXs.Length; i++)
            {
                Instantiate(impactVFXs[i], transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
            }
        }
    }
}
