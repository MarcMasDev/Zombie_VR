using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] zombieWalkerPrefabs;
    [SerializeField] private GameObject[] zombieRunnerPrefabs;

    public float spawnDelay = 2f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnDelay && GameManager.Instance.CanSpawnZombie())
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    private void SpawnZombie()
    {
        Instantiate(GetRandomSpeedPrefab(), transform.position, Quaternion.identity).GetComponent<Health>();

        GameManager.Instance.RegisterZombie();
    }

    private GameObject GetRandomSpeedPrefab()
    {
        if (Random.value < GameManager.Instance.GetZombieRunnerChance())
        {
            return zombieRunnerPrefabs[Random.Range(0, zombieRunnerPrefabs.Length)];
        }

        return zombieWalkerPrefabs[Random.Range(0, zombieWalkerPrefabs.Length)];
    }
}
