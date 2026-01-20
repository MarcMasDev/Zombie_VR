using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AmmoIdentifier
{
    public WeaponID id;
    public Transform parent;
    public RectTransform ammoVisualizer; 
    public TMP_Text scoreDisplayer;
    public Image fill;
}
public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    [SerializeField] private BeltAmmo belt;
    [SerializeField] private AmmoIdentifier ammoID1;
    [SerializeField] private AmmoIdentifier ammoID2;
    [SerializeField] private float size  = 0.0004f;
    void Awake()
    {
        Instance = this;
    }
    public void UpdateAmmoManager(float currentAmmo, float maxAmmo, WeaponID weaponUniqueID, Transform place = null)
    {
        //if (ammoID1.id == weaponUniqueID)
        //{
        //    SetInfo(currentAmmo, maxAmmo, weaponUniqueID);
        //    return;
        //}
        //if (ammoID2.id == weaponUniqueID)
        //{
        //    SetInfo(currentAmmo, maxAmmo, weaponUniqueID);
        //    return;
        //}

        //if (place != null)
        //{
        //    if (ammoID1.id == null)
        //    {
        //        ammoID1.id = weaponUniqueID;
        //        ammoID1.parent = place;
        //        SetUpAmmoManager(ammoID1, place);
        //    }
        //    else if (ammoID2.id == null)
        //    {
        //        ammoID2.id = weaponUniqueID;
        //        ammoID2.parent = place;
        //        SetUpAmmoManager(ammoID2, place);
        //    }
        //}

        //SetInfo(currentAmmo, maxAmmo, weaponUniqueID);
    }
    public void ResetParent(WeaponID weaponUniqueID)
    {
        //if (ammoID1.id == weaponUniqueID)
        //{
        //    ResetAmmo(ammoID1);
        //    belt.ResetAmmo(1);
        //}
        //else if (ammoID2.id == weaponUniqueID)
        //{
        //    ResetAmmo(ammoID2);
        //    belt.ResetAmmo(2);
        //}
    }

    private void SetUpAmmoManager(AmmoIdentifier ammoUI, Transform parent)
    {
        //ammoUI.ammoVisualizer.gameObject.SetActive(true);
        //ammoUI.ammoVisualizer.SetParent(parent);
        //ammoUI.ammoVisualizer.localPosition = Vector3.zero;
        //ammoUI.ammoVisualizer.localRotation = Quaternion.identity;
        //ammoUI.ammoVisualizer.localScale = new Vector3(size, size, size);
    }
    private void ResetAmmo(AmmoIdentifier ammoUI)
    {
        //ammoUI.id = null;
        //ammoUI.parent = null;

        //ammoUI.ammoVisualizer.SetParent(null);
        //ammoUI.ammoVisualizer.gameObject.SetActive(false);
    }
    private void SetAmmo(float currentAmmo, float maxAmmo, AmmoIdentifier ammoUI)
    {
        //ammoUI.scoreDisplayer.text = currentAmmo.ToString("N0");
        //ammoUI.fill.fillAmount = currentAmmo / (float)maxAmmo;
    }
    private void SetInfo(float currentAmmo, float maxAmmo, WeaponID weaponUniqueID)
    {
        //if (ammoID1.id == weaponUniqueID)
        //{
        //    SetAmmo(currentAmmo, maxAmmo, ammoID1);
        //    belt.UpdateAmmo(weaponUniqueID, 1);
        //}
        //else if (ammoID2.id == weaponUniqueID)
        //{
        //    SetAmmo(currentAmmo, maxAmmo, ammoID2);
        //    belt.UpdateAmmo(weaponUniqueID, 2);
        //}
    }
}
