using System;
using UnityEngine;

namespace Combat
{
    public abstract class Entity : MonoBehaviour
    {
        public bool IsDead {get; private set;}
        
        protected virtual void Start()
        {
            InitializeEntity();
        }

        protected virtual void InitializeEntity()
        {
            Health health = GetComponent<Health>();
            if (health is null)
            {
                Debug.LogError("Health is null", this);
                return;
            }
            
            health.Init(GetMaxHealth());
            health.OnDamaged += HealthOnDamaged;
            health.OnDeath += HealthOnDeath;
        }

        protected void SetIsDead() => IsDead = true;
        protected abstract void HealthOnDamaged(float damageAmount);
        protected abstract void HealthOnDeath();
        protected abstract int GetMaxHealth();
    }
}
