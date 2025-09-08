using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/RangedAttack")]
public class RangedAttackData : AttackData
{
    [Header("Ranged Specific")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;
    public float projectileLifetime = 5f;
}
