using UnityEngine;

public class EnableFakeDoor : MonoBehaviour
{
    [SerializeField] private Animator open;
    [SerializeField] private Material correctLampMat;
    [SerializeField] private Material incorrectLampMat;
    [SerializeField] private MeshRenderer[] lamps;
    [SerializeField] private BeltAmmo belt;
    [SerializeField] private GameObject datafono;
    private int[] scores = new int[4];
    private bool gameStarted = false;

    private void Update()
    {
        if (scores[3] != 0) return ;
        if (belt.magazinesOnBelt.Count > 0)
        {
            AddScore(3);
        }
        if (!datafono.activeSelf && IsDone())
        {
            datafono.SetActive(true);
        }
    }
    public void AddScore(int scoreType)
    {
        scores[scoreType] = 1;
        lamps[scoreType].material = correctLampMat;
    }
    public void RemoveScore(int scoreType)
    {
        scores[scoreType] = 0;
        lamps[scoreType].material = incorrectLampMat;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !gameStarted)
        {
            gameStarted = true;
            StartGame();
        }
    }
    private void StartGame()
    {
        GameManager.Instance.StartNextRound();
        open.SetTrigger("Close");
    }
    private bool IsDone()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == 0) return false;
        }
        return true;
    }
}
