using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private TMP_Text scoreDisplayer;

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
        int pointsMultiplier = 1;
        if (doublePoints) pointsMultiplier = 2;

        if (!death) points += pointsPerImpact * pointsMultiplier;
        else
        {
            switch (hitboxType)
            {
                case HitboxType.Head:
                    points += pointsPerHeadshot * pointsMultiplier;
                    break;
                default:
                    points += pointsPerKill * pointsMultiplier;
                    break;
            }
        }

        scoreDisplayer.text = points.ToString("N0");
    }

}
