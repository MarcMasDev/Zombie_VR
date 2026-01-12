using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text roundVisualizer;

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

    private int round = 0;
    public int GetRound() => round;

    private int zombiesSpawnedThisRound;
    private int zombiesThisRound;
    private int zombiesAlive;
    void Awake()
    {
        Instance = this;
    }

    public bool CanSpawnZombie()
    {
        return zombiesAlive < maxAliveZombies && zombiesSpawnedThisRound < zombiesThisRound;
    }

    public void RegisterZombie()
    {
        zombiesAlive++;
        print("Hey! I'm registering a new zombie. The total of zombies alive is: " + zombiesAlive);

        zombiesSpawnedThisRound++;
        print("And remember I can spawn up to " + zombiesThisRound + " right now I have spawned " + zombiesSpawnedThisRound + " already!");
    }

    public void UnregisterZombie()
    {
        zombiesAlive--;
        print("Nice! You killed a zombie. The total of zombies alive is: " + zombiesAlive);
    }
    public int StartNextRound()
    {
        if ((zombiesSpawnedThisRound >= zombiesThisRound && zombiesAlive <= 0) ||round == 0)
        {
            print("Hi! I'm starting a new round because you activated the trigger and: /n " +
                "Zombies spawned this round (" + zombiesSpawnedThisRound + ") is greater or equal to the zombies I had to spawn (" + zombiesThisRound + ") /n" +
                "And guess what! The zombies alive (" + zombiesAlive + ") was lower than 0!!! This means I can start a new round");
            round++;
            zombiesSpawnedThisRound = 0;
            roundVisualizer.text = round.ToString("N0");
            CalculateZombiesThisRound(); 
            return round;
        }

        return -1;
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
