using TMPro;
using UnityEngine;

public class SetRoundVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Animator animator;

    public void StartRound()
    {
        int currentRound = GameManager.Instance.StartNextRound();

        if (currentRound == -1)
        {
            animator.SetBool("Condition", false);
        }
        else
        {
            animator.SetBool("Condition", true);
            roundText.text = currentRound.ToString();
        }
    }
}
