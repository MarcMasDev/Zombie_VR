using System.Collections;
using UnityEngine;

public class WeaponRandomizer : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject[] weaponsVisuals;
    [SerializeField] private float timeBetweenWeapons = 0.25f;
    [SerializeField] private int maxWeaponsToShow = 25;


    [Header("Spawn")]
    [SerializeField] private GameObject[] weaponsToSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnForce = 5f;

    private int index = 0;
    private bool randomizing = false;
    public void StartRandomize()
    {
        randomizing = true;
        index = Random.Range(0, weaponsVisuals.Length);
        StartCoroutine(ShowRandomWeapons());
    }
    public void EndRandomize()
    {
        randomizing = false;
        SpawnRandomWeapon();
    }

    private float weaponsShown = 0;
    private IEnumerator ShowRandomWeapons()
    {
        weaponsShown = 0;
        while (randomizing && weaponsShown < maxWeaponsToShow)
        {
            weaponsVisuals[index].SetActive(true);

            yield return new WaitForSeconds(timeBetweenWeapons);

            weaponsVisuals[index].SetActive(false);
            index = (index + 1) % weaponsVisuals.Length;
            weaponsShown++;
        }
    }
    private void SpawnRandomWeapon()
    {
        Rigidbody weapon = Instantiate(weaponsToSpawn[Random.Range(0, weaponsToSpawn.Length)], 
            spawnPoint.position, spawnPoint.rotation).GetComponent<Rigidbody>();

        weapon.linearVelocity = spawnPoint.forward * spawnForce;
    }
}
