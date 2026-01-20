using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoParent : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text currentAmmoText;
    [SerializeField] private Image fill;

    public void SetColliders(bool enabled)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = enabled;
        }
        canvas.SetActive(enabled);
    }

    public void SetAmmo(int currentAmmo, int maxAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString("N0");
        fill.fillAmount = currentAmmo / (float)maxAmmo;
    }

    private float lockedWorldZ = 0f;
    private Transform parent;

    void Awake()
    {
        parent = transform.parent;
    }

    void LateUpdate()
    {
        Vector3 worldEuler = transform.rotation.eulerAngles;
        worldEuler.z = lockedWorldZ;

        Quaternion lockedWorldRotation = Quaternion.Euler(worldEuler);

        if (parent != null)
        {
            transform.localRotation =
                Quaternion.Inverse(parent.rotation) * lockedWorldRotation;
        }
        else
        {
            transform.rotation = lockedWorldRotation;
        }
    }
}
