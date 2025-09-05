using System;
using UnityEngine;

public struct HitInfo
{
    public GameObject Source;
    public float DamageAmount;
}

public interface IDamageable
{
    public void TakeDamage(HitInfo hitInfo);
}

public class Health : MonoBehaviour, IDamageable
{
    public float MaxHealth { get; private set; }
    private float CurrentHealth { get; set; }

    public event Action<float> OnDamaged;
    public event Action OnDeath;

    public void Init(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(HitInfo hitInfo)
    {
        CurrentHealth -= hitInfo.DamageAmount;
        OnDamaged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
