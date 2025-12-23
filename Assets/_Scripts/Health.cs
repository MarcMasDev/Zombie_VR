using UnityEngine;
public interface IDamageable
{
    void TakeDamage(float amount, HitboxType hitbox);
}

public class Health : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private RagdollAgent ragdoll;

    private void Awake()
    {
        maxHealth = GameManager.Instance.GetZombieHealth();
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        ragdoll = GetComponent<RagdollAgent>();
    }

    public void TakeDamage(float amount, HitboxType hitbox)
    {
        currentHealth -= amount;
        bool death = currentHealth <= 0;


        ScoreManager.Instance.AddPoints(hitbox, death);

        if (death)
        {
            Die();
            return;
        }

        if (animator) animator.SetTrigger("Hit");
    }

    private void Die()
    {
        ragdoll.EnableRagdoll();
        GameManager.Instance.UnregisterZombie();
    }
}
