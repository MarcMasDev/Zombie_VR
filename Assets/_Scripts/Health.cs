using UnityEngine;
public interface IDamageable
{
    void TakeDamage(float amount);
}

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private RagdollAgent ragdoll;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        ragdoll = GetComponent<RagdollAgent>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (animator) animator.SetTrigger("Hit");
    }

    private void Die()
    {
        ragdoll.EnableRagdoll();
    }
}
