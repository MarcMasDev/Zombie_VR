using UnityEngine;

public enum FireMode
{
    SemiAuto,
    FullAuto,
    Burst
}

[CreateAssetMenu(fileName = "WeaponClass", menuName = "Scriptable Objects/WeaponClass")]
public class WeaponClass : ScriptableObject
{
    [Header("Combat")]
    public float damage = 25f;
    public float projectileSpeed = 100f;
    public float headshotMultiplier = 2f;

    [Header("Fire")]
    public FireMode fireMode = FireMode.SemiAuto;
    [Tooltip("Shots per second (used for FullAuto / Burst)")]
    public float fireRate = 10f;
    public int burstCount = 3;

    [Header("Ammo")]
    public int magazineSize = 30;
    public float reloadTime = 2f;
}
