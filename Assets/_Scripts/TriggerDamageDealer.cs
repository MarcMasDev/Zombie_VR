using UnityEngine;

public class TriggerDamageDealer : MonoBehaviour
{
    [SerializeField] private float damage = 100;
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage, HitboxType.Body);
        }

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamager((int)damage/2);
        }
    }
}
