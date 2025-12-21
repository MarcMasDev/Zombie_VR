using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponClass equippedWeapon;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform firePoint;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    public void ShootWeapon()
    {
        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, transform.rotation);
        projectile.Fire(equippedWeapon, firePoint.forward);
    }
}
