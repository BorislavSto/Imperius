using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/MeleeAttack")]
public class MeleeAttackData : AttackData
{
    [Header("Melee Specific")]
    public float hitboxActiveTime = 0.2f;
}
