using TMPro;
using UnityEngine;

public class Purchasable : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text costDisplay;

    [SerializeField] private Animator[] anim;


    private void Awake()
    {
        costDisplay.text = cost.ToString("N0");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Watch"))
        {
            bool checkPurchase = ScoreManager.Instance.CheckAndApplyAmount(cost);
            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetBool("Purchase", checkPurchase);
                anim[i].SetTrigger("TryPurchase");
            }
        }
    }
}
