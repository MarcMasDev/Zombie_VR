using UnityEngine;

public class SpawnOnExit : MonoBehaviour
{
    [SerializeField] private GameObject g;
    private void Awake()
    {
        if (g != null) Instantiate(g, transform.position, Quaternion.identity);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AmmoCounter")) if (g != null) Instantiate(g, transform.position, Quaternion.identity);

    }
}
