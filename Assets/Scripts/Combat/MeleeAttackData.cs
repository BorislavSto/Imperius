using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/MeleeAttack")]
public class MeleeAttackData : AttackData
{
    [Header("Melee Specific")]
    public float hitboxActiveTime = 0.2f;
    
    public override Attack CreateAttack(AttackHandler attackHandler)
    {
        if (!attackHandler.DmgRelay || !attackHandler.DamageArea)
        {
            Debug.LogError("MeleeAttackData requires DamageRelay + DamageArea on the AttackHandler!");
            return null;
        }
        
        return new MeleeAttack(this, attackHandler.DmgRelay, attackHandler.DamageArea);
    }
}
