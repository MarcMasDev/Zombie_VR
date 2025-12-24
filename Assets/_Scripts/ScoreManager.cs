using TMPro;
using UnityEngine;
using DamageNumbersPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private TMP_Text scoreDisplayer;
    [SerializeField] private DamageNumber floatingText;
    [SerializeField] private Transform numberSpawnPoint;

    [Header("Score Settings")]
    [SerializeField] private int pointsPerImpact = 10;
    [SerializeField] private int pointsPerHeadshot = 100;
    [SerializeField] private int pointsPerKill = 50;

    public bool doublePoints = false;
    private int points = 0;
    void Awake()
    {
        Instance = this;
    }
    public void AddPoints(HitboxType hitboxType, bool death = false)
    {
        int currentPoints = 0;
        int pointsMultiplier = 1;
        if (doublePoints) pointsMultiplier = 2;

        if (!death) currentPoints = pointsPerImpact * pointsMultiplier;
        else
        {
            switch (hitboxType)
            {
                case HitboxType.Head:
                    currentPoints = pointsPerHeadshot * pointsMultiplier;
                    break;
                default:
                    currentPoints = pointsPerKill * pointsMultiplier;
                    break;
            }
        }

        points += currentPoints;
        scoreDisplayer.text = points.ToString("N0");
        SpawnPoints(currentPoints);
    }

    private void SpawnPoints(int pointsToShow)
    {
        floatingText.Spawn(numberSpawnPoint.position, pointsToShow).transform.SetParent(numberSpawnPoint);

    }
}
