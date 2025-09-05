using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/RangedAttack")]
public class RangedAttackAsset : AttackAsset
{
    [Header("Ranged Specific")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;
    public float projectileLifetime = 5f;
    
    public override IAttackBehavior CreateBehavior(Attack executor)
    {
        return new RangedAttackBehavior(this, (RangedAttack)executor);
    }
}
