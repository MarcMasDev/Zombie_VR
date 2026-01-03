using UnityEngine;
using UnityEngine.Audio;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    public void TryDealDamageToPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamager(damageAmount);
                AudioClip clip = clips[Random.Range(0, clips.Length)];
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
