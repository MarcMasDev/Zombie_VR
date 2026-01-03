using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    [SerializeField] private TMP_Text scoreDisplayer;
    [SerializeField] private Image fill;
    [SerializeField] private RectTransform ammoVisualizer;
    [SerializeField] private float size  = 0.0004f;
    void Awake()
    {
        Instance = this;
        ammoVisualizer.gameObject.SetActive(false);
    }
    public void UpdateAmmoManager(float currentAmmo, float maxAmmo, bool show = true, Transform place = null)
    {
        if (show)
        {
            if (place != null)
            {
                ammoVisualizer.gameObject.SetActive(true);
                ammoVisualizer.SetParent(place);
                ammoVisualizer.localPosition = Vector3.zero;
                ammoVisualizer.localRotation = Quaternion.identity;
                ammoVisualizer.localScale = new Vector3(size, size, size);
            }

            if (ammoVisualizer.gameObject.activeSelf) SetInfo(currentAmmo, maxAmmo);
        }
        else ammoVisualizer.gameObject.SetActive(false);

    }
    public void ResetParent()
    {
        ammoVisualizer.SetParent(null);
        ammoVisualizer.gameObject.SetActive(false);
    }
    private void SetInfo(float currentAmmo, float maxAmmo)
    {
        scoreDisplayer.text = currentAmmo.ToString("N0");
        fill.fillAmount = currentAmmo / (float)maxAmmo;
    }
}
