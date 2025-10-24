using System;
using UnityEngine;

namespace Combat
{
    public abstract class Entity : MonoBehaviour
    {
        public bool IsDead {get; private set;}
        protected Health Health {get ; private set;}
        
        protected virtual void Start()
        {
            InitializeEntity();
        }

        protected virtual void InitializeEntity()
        {
            Health = GetComponent<Health>();
            if (Health is null)
            {
                Debug.LogError("Health is null", this);
                return;
            }
            
            Health.Init(SetMaxHealthInHealth());
            Health.OnDamaged += HealthOnDamaged;
            Health.OnDeath += HealthOnDeath;
        }

        protected void SetIsDead() => IsDead = true;
        protected abstract void HealthOnDamaged(float damageAmount);
        protected abstract void HealthOnDeath();
        protected abstract int SetMaxHealthInHealth();
    }
}
