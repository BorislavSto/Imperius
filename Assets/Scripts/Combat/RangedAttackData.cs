using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(menuName = "Attacks/RangedAttack")]
    public class RangedAttackData : AttackData
    {
        [Header("Ranged Specific")] public GameObject projectilePrefab;
        public float projectileSpeed = 15f;
        public float projectileLifetime = 5f;

        [Header("Multi-Shot")] public int projectileCount = 1;
        public float spreadAngle;

        public override Attack CreateAttack(AttackHandler attackHandler)
        {
            if (!attackHandler.ShootOrigin)
            {
                Debug.LogError("RangedAttackData requires ShootOrigin on the AttackHandler!");
                return null;
            }

            return new RangedAttack(this, attackHandler.ShootOrigin);
        }
    }
}
