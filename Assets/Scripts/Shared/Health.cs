using System;
using UnityEngine;

public class Health : MonoBehaviour
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

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        OnDamaged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
