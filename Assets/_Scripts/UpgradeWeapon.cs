using UnityEngine;

public class UpgradeWeapon : MonoBehaviour
{
    [SerializeField] private Upgrader u;
    public void StartUpgrade()
    {
        u.StartUpgradeWeapon();
    }
    public void Upgrade()
    {
        u.UpgradeWeapon();
    }
}
