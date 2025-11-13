using System;
using UnityEngine;

namespace Combat
{
    public abstract class Entity : MonoBehaviour
    {
        protected Health health {get ; private set;}
        
        protected virtual void Start()
        {
            InitializeEntity();
        }

        protected virtual void InitializeEntity()
        {
            health = GetComponent<Health>();
            if (health is null)
            {
                Debug.LogError("Health is null", this);
                return;
            }
            
            health.Init(SetMaxHealthInHealth());
            health.OnDamaged += HealthOnDamaged;
            health.OnDeath += HealthOnDeath;
        }
        
        protected void SetIsDead() => health.SetIsDead();
        protected abstract void HealthOnDamaged(float damageAmount);
        protected abstract void HealthOnDeath();
        protected abstract int SetMaxHealthInHealth();
    }
}
