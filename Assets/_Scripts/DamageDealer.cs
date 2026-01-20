using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class HitSound
{
    public AudioSource audioSource;
    public AudioClip[] clips;
}
public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private HitSound[] hitAudio;
    public void TryDealDamageToPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamager(damageAmount);
                for (int i = 0; i < hitAudio.Length; i++)
                {
                    AudioClip clip = hitAudio[i].clips[Random.Range(0, hitAudio[i].clips.Length)];
                    hitAudio[i].audioSource.PlayOneShot(clip);
                }
            }
        }
    }
}
