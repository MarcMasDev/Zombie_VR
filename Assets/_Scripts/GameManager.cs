using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base Values")]
    [SerializeField] private int baseZombiesPerRound = 20;
    [SerializeField] private int maxAliveZombies = 5;
    [SerializeField] private float baseZombieHealth = 100f;
    [SerializeField] private float baseRunnerChance = 0;

    [Header("Growth")]
    [SerializeField] private float healthGrowthMultiplier = 1.25f;
    [SerializeField] private float amountGrowthMultiplier = 1.2f; 
    [SerializeField] private float runnerChanceGrowthMultiplier = 1.2f; 
    [SerializeField] private float maxRunnerChance = 0.9f; 


    private int round = 1;
    private int zombiesSpawnedThisRound;
    private int zombiesThisRound;
    private int zombiesAlive;
    void Awake()
    {
        Instance = this;
        StartNextRound();
    }

    public bool CanSpawnZombie()
    {
        return zombiesAlive < maxAliveZombies;
    }

    public void RegisterZombie()
    {
        zombiesAlive++;
        zombiesSpawnedThisRound++;
    }
    public void UnregisterZombie()
    {
        zombiesAlive--;

        if (zombiesSpawnedThisRound >= zombiesThisRound && zombiesAlive <= 0)
        {
            StartNextRound();
        }
    }
    private void StartNextRound()
    {
        round++;
        zombiesSpawnedThisRound = 0;
        CalculateZombiesThisRound();
    }

    private void CalculateZombiesThisRound()
    {
        zombiesThisRound = Mathf.RoundToInt(baseZombiesPerRound * Mathf.Pow(amountGrowthMultiplier, round - 1));
    }

    public float GetZombieHealth()
    {
        return baseZombieHealth * Mathf.Pow(healthGrowthMultiplier, round - 1);
    }
    public float GetZombieRunnerChance()
    {
        return Mathf.Min(baseRunnerChance * Mathf.Pow(runnerChanceGrowthMultiplier, round - 1), maxRunnerChance);
    }
}
