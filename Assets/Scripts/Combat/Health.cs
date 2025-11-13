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
        private int maxHealth;
        public int currentHealth { get; private set; }
        
        public event Action<float> OnDamaged;
        public event Action OnDeath;
        
        public bool isDead { get; private set; }

        public void Init(int maxHp)
        {
            maxHealth = maxHp;
            currentHealth = maxHp;
        }

        public void TakeDamage(HitInfo hitInfo)
        {
            currentHealth -= hitInfo.DamageAmount;
            OnDamaged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();
                isDead = true;
            }
        }
        
        public void Heal(int amount)
        {
            if (currentHealth + amount > maxHealth)
                currentHealth = maxHealth;
            else 
                currentHealth += amount;
        }
        
        public void SetIsDead() => isDead = true;
        public void SetIsAlive() => isDead = false;
    }
}
