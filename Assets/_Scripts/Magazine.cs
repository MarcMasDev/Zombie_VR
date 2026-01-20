using UnityEngine;

public class Magazine : MonoBehaviour
{
    private int currentAmmo;
    public WeaponClass weaponClass;
    public WeaponID weaponID;
    public int CurrentAmmo => currentAmmo;

    private void Awake()
    {
        currentAmmo = weaponClass.magazineSize;
    }

    public bool HasAmmo()
    {
        return currentAmmo > 0;
    }

    public bool ConsumeBullet()
    {
        if (currentAmmo <= 0)
            return false;

        currentAmmo--;
        return true;
    }
    public void DestroyMagazine()
    {
        if (currentAmmo <= 0) Destroy(gameObject, 1);
    }
}
