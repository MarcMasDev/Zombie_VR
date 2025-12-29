using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;

    [SerializeField] private GameObject[] zombieWalkerPrefabs;
    [SerializeField] private GameObject[] zombieRunnerPrefabs;

    [SerializeField] private Vector2 randomSpawnDelay;
    private float spawnDelay;
    private float timer;

    private void Start()
    {
        SetRandomSpawntime();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnDelay && GameManager.Instance.CanSpawnZombie())
        {
            SetRandomSpawntime();
            SpawnZombie();
            timer = 0f;
        }
    }

    private void SpawnZombie()
    {
        Instantiate(GetRandomSpeedPrefab(), GetRandomSpawnPoint(), Quaternion.identity).GetComponent<Health>();

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
    private Vector3 GetRandomSpawnPoint()
    {
        return spawners[Random.Range(0, spawners.Length)].position;
    }
    private void SetRandomSpawntime()
    {
        spawnDelay = Random.Range(randomSpawnDelay.x, randomSpawnDelay.y);
    }
}
