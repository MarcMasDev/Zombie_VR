using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float m_Time = 3;

    private void Awake()
    {
        Destroy(gameObject, m_Time);
    }
}
