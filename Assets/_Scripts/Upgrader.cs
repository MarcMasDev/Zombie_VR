using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    private List<WeaponID> weaponsInZone = new List<WeaponID>();
    private WeaponID CurrentWeapon => weaponsInZone.Count > 0 ? weaponsInZone[weaponsInZone.Count - 1] : null;

    [SerializeField] private GameObject[] weapons;
    [SerializeField] private Purchasable purchaseChecker;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Weapon")) return;

        WeaponID weapon = other.GetComponent<WeaponID>();
        if (weapon == null) return;

        if (!weaponsInZone.Contains(weapon))
            weaponsInZone.Add(weapon);

        purchaseChecker.waitForAction = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Weapon")) return;

        WeaponID weapon = other.GetComponent<WeaponID>();
        if (weapon == null) return;

        weaponsInZone.Remove(weapon);

        if (weaponsInZone.Count == 0)
            purchaseChecker.waitForAction = true;
    }

    public void StartUpgradeWeapon()
    {
        WeaponID weaponToUpgrade = CurrentWeapon;
        if (weaponToUpgrade == null) return;
        weaponToUpgrade.gameObject.SetActive(false);
    }
    public void UpgradeWeapon()
    {
        WeaponID weaponToUpgrade = CurrentWeapon;
        int id = weaponToUpgrade.id;
        Vector3 pos = weaponToUpgrade.transform.position;
        Quaternion rot = weaponToUpgrade.transform.rotation;

        weaponsInZone.Remove(weaponToUpgrade);
        Instantiate(weapons[id], pos, rot);
        Destroy(weaponToUpgrade.gameObject);

        if (weaponsInZone.Count == 0)
            purchaseChecker.waitForAction = true;
    }
}
