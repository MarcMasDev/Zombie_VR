using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private LayerMask playerLayer;
    public void TryDealDamageToPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamager(damageAmount);
            }
        }
    }
}
