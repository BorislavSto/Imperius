using Combat;
using UnityEngine;

namespace Player
{
    public class PlayerCharacter : Entity
    {
        protected override void HealthOnDamaged(float obj)
        {
            Debug.Log("PlayerCharacter HealthOnDamaged");
        }

        protected override void HealthOnDeath()
        {
            Debug.Log("PlayerCharacter HealthOnDeath");
        }

        protected override int GetMaxHealth() => 30;
    }
}
