using UnityEngine;

namespace Combat
{
    public abstract class Entity : MonoBehaviour
    {
        protected virtual void Start()
        {
            InitializeEntity();
        }

        protected virtual void InitializeEntity()
        {
            Health health = GetComponent<Health>();
            health?.Init(GetMaxHealth());
        }

        protected abstract int GetMaxHealth();
    }
}
