using UnityEngine;
using UnityEngine.UI;

public class SetRoundVisualizer : MonoBehaviour
{
    private Text t;
    private void Start()
    {
        t = GetComponent<Text>(); 
        if (t != null) t.text = "Round: " + GameManager.Instance.GetRound().ToString("N0");
    }
    private void OnEnable()
    {
        if (t != null) t.text = "Round: " + GameManager.Instance.GetRound().ToString("N0");
    }
    private void Update()
    {
        if (t != null) t.text = "Round: " + GameManager.Instance.GetRound().ToString("N0");
    }
}
