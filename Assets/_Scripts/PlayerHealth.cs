using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using AmplifyColor;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private float regenerationSpeed = 25;
    [SerializeField] private float timeToRegenerate = 1;
    [SerializeField] private WatchGrab watch;
    private float currentPlayerHealth;
    private float regenerateAllowTimer = 1;
    [SerializeField] private Image fill;

    [Header("Post Processing")]
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private PostProcessVolume postProcessVolume2;
    [SerializeField] private float minPPweight = 0f;
    [SerializeField] private float maxPPweight = 1f;


    private void Awake()
    {
        currentPlayerHealth = playerHealth;
        UpdateVisuals();
    }

    private void Update()
    {
        Regenerate();
        UpdateVisuals();
    }

    private void Regenerate()
    {
        if (currentPlayerHealth < playerHealth)
        {
            regenerateAllowTimer -= Time.deltaTime;
            if (regenerateAllowTimer < 0)
            {
                currentPlayerHealth += regenerationSpeed;
                currentPlayerHealth = Mathf.Min(currentPlayerHealth, playerHealth);
                regenerateAllowTimer = timeToRegenerate;
            }
        }
    }

    public void TakeDamager(int damage)
    {
        currentPlayerHealth -= damage;
        currentPlayerHealth = Mathf.Max(currentPlayerHealth, 0);
        regenerateAllowTimer = timeToRegenerate;
        UpdateVisuals();

        if (currentPlayerHealth <= 0)
            GameOver();
    }
    private void GameOver()
    {
        postProcessVolume.weight = 0;
        postProcessVolume2.weight = 1;
        watch.GO();
        Time.timeScale = 0;
    }
    private void UpdateVisuals()
    {
        fill.fillAmount = currentPlayerHealth / (float)playerHealth; 
        
        //Post process
        float healthPercent = currentPlayerHealth / (float)playerHealth;
        float dangerFactor = 1f - healthPercent;

        postProcessVolume.weight = Mathf.Lerp(minPPweight, maxPPweight, dangerFactor);
    }
}
