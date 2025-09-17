using System;
using UnityEngine;

namespace Combat
{
    public struct HitInfo
    {
        public GameObject Source;
        public int DamageAmount;
    }

    public interface IDamageable
    {
        public void TakeDamage(HitInfo hitInfo);
    }

    public class Health : MonoBehaviour, IDamageable
    {
        public int MaxHealth { get; private set; }
        private int CurrentHealth { get; set; }

        public event Action<float> OnDamaged;
        public event Action OnDeath;

        public void Init(int maxHealth)
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
}
