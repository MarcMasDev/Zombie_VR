using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealth = 100;
    private int currentPlayerHealth;
    [SerializeField] private Image fill;

    private void Awake()
    {
        currentPlayerHealth = playerHealth;
        UpdateVisuals();
    }
    public void TakeDamager(int damage)
    {
        currentPlayerHealth -= damage;
        UpdateVisuals();
    }
    private void GameOver()
    {

    }
    private void UpdateVisuals()
    {
        fill.fillAmount = currentPlayerHealth / (float)playerHealth;
    }
}
