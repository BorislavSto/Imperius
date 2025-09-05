using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/MeleeAttack")]
public class MeleeAttackAsset : AttackAsset
{
    [Header("Melee Specific")]
    public float hitboxActiveTime = 0.2f;

    public override IAttackBehavior CreateBehavior(Attack executor)
    {
        return new MeleeAttackBehavior(this, (MeleeAttack)executor);
    }
}
