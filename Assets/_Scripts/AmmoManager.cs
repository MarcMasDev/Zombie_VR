using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    [SerializeField] private TMP_Text scoreDisplayer;
    [SerializeField] private Image fill;
    void Awake()
    {
        Instance = this;
    }
    public void UpdateAmmoManager(int currentAmmo, int maxAmmo)
    {
        scoreDisplayer.text = currentAmmo.ToString("N0");
        fill.fillAmount = currentAmmo / (float)maxAmmo;
    }
}
