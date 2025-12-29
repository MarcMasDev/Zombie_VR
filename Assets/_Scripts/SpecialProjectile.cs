using UnityEngine;

public class SpecialProjectile : Projectile
{
    private WeaponClass weapon;

    [Header("Blood VFX")]
    [SerializeField] private GameObject[] bloodVFXs;

    [Header("Impact VFX")]
    [SerializeField] private GameObject[] impactVFXs;

    public override void Fire(WeaponClass weapon, Vector3 dir)
    {
        this.weapon = weapon;
    }
}
