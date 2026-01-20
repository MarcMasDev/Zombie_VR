using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class BeltAmmo : MonoBehaviour
{
    public static BeltAmmo Instance;

    [HideInInspector] public List <Magazine> magazinesOnBelt = new List<Magazine>();
    [SerializeField] private TMP_Text currentEquippedWeaponAmmoText;
    [SerializeField] private TMP_Text globalAmmoText;
    private int globalAmmo = 0;
    private int rightHandWeaponID = -1, leftHandWeaponID = -1;

    private void Awake()
    {
        Instance = this;
        currentEquippedWeaponAmmoText.text = "";
    }
    public void SetAmmo(bool grabbed, WeaponID weaponUniqueID, bool leftHand)
    {
        if (leftHand)
        {
            if (grabbed) leftHandWeaponID = weaponUniqueID.id;
            else leftHandWeaponID = -1;

            UpdateLocalVisuals();
        }
        else
        {
            if (grabbed) rightHandWeaponID = weaponUniqueID.id;
            else rightHandWeaponID = -1;

            UpdateLocalVisuals();
        }


    }
    public void AddMagazine(SelectEnterEventArgs m)
    {
        Magazine magazine = m.interactableObject.transform.gameObject.GetComponent<Magazine>();
        magazinesOnBelt.Add(magazine);
        globalAmmo += magazine.CurrentAmmo;
        UpdateGlobalVisuals();
        UpdateLocalVisuals();
    }
    public void RemoveMagazine(SelectExitEventArgs m)
    {
        Magazine magazine = m.interactableObject.transform.gameObject.GetComponent<Magazine>();
        magazinesOnBelt.Remove(magazine);

        globalAmmo -= magazine.CurrentAmmo;
        if (globalAmmo < 0) globalAmmo = 0; //por si acaso

        UpdateLocalVisuals();
        UpdateGlobalVisuals();
    }

    private void UpdateGlobalVisuals()
    {
        globalAmmoText.text = globalAmmo.ToString("N0");
    }
    private void UpdateLocalVisuals()
    {
        int rightHandAmmo = 0; int leftHandAmmo = 0;

        for (int i = 0; i < magazinesOnBelt.Count; i++)
        {
            if (rightHandWeaponID == magazinesOnBelt[i].weaponID.id) rightHandAmmo += magazinesOnBelt[i].CurrentAmmo;
            if (leftHandWeaponID == magazinesOnBelt[i].weaponID.id) leftHandAmmo += magazinesOnBelt[i].CurrentAmmo;
        }

        if (rightHandWeaponID == -1)
        {
            //No weapons
            if (leftHandWeaponID == -1)
            {
                currentEquippedWeaponAmmoText.text = "";
            }
            //only left
            else
            {
                currentEquippedWeaponAmmoText.text = leftHandAmmo.ToString("N0");
            }
        }
        else if (leftHandWeaponID == -1)
        {
            //only right
            currentEquippedWeaponAmmoText.text = rightHandAmmo.ToString("N0");
        }
        else
        {
            currentEquippedWeaponAmmoText.text = leftHandAmmo.ToString("N0") + " | " + rightHandAmmo.ToString("N0");
        }
    }
}
