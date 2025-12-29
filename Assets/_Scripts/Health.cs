using System.Collections;
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

    //Fire
    [SerializeField] private ParticleSystem fireEffect;
    [HideInInspector] public bool fire = false;
    [SerializeField] private float fireDuration = 3f;
    private float noFireDuration = 0;
    private float fireDamagePercentage = 0;
    private float fireTickTimer = 0;
    private void Awake()
    {
        maxHealth = GameManager.Instance.GetZombieHealth();
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        ragdoll = GetComponent<RagdollAgent>();
    }
    public void TakeDamage(float amount, HitboxType hitbox)
    {
        if (hitbox == HitboxType.Fire && !isDeath)
        {
            noFireDuration = 0;
            fireDamagePercentage = amount;
            fire = true;
            return;
        }

        if (fire) currentHealth -= maxHealth * (amount / 100f);
        else currentHealth -= amount;


        bool death = currentHealth <= 0;

        ScoreManager.Instance.AddPoints(hitbox, death);

        if (death)
        {
            Die();
            return;
        }

        if (animator) animator.SetTrigger("Hit");
    }

    private bool isDeath = false;
    private void Die()
    {
        fireEffect.Stop();
        if (!isDeath)
        {
            isDeath = true;
            ragdoll.EnableRagdoll();
            GameManager.Instance.UnregisterZombie();
        }
    }

    private void Update()
    {
        if (fire) HandleFire();
    }
    private void HandleFire()
    {
        if (!fireEffect.isPlaying)
            fireEffect.Play();

        fireTickTimer += Time.deltaTime;
        noFireDuration += Time.deltaTime;
        if (fireTickTimer >= 1f)
        {
            fireTickTimer = 0f;
            TakeDamage(fireDamagePercentage, HitboxType.Body);
        }

        if (noFireDuration > fireDuration)
        {
            fire = false;
            fireEffect.Stop();
        }
    }
}
