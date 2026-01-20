using TMPro;
using UnityEngine;

public class Purchasable : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text costDisplay;

    [SerializeField] private Animator[] anim;
    [SerializeField] private bool onlyOnce = false;
    private bool onlyOnceTrigger = false;
    public bool waitForAction = false;

    private void Awake()
    {
        costDisplay.text = cost.ToString("N0");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Watch"))
        {
            if (waitForAction) return;

            bool checkPurchase = ScoreManager.Instance.CheckAndApplyAmount(cost);
            SetAnimators(checkPurchase);
        }
    }
    public void SetAnimators(bool purchased)
    {
        if (onlyOnce && onlyOnceTrigger) return;
        else onlyOnceTrigger = true;

        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetBool("Purchase", purchased);
            if (purchased) anim[i].SetTrigger("TryPurchase");
        }
    }
}
