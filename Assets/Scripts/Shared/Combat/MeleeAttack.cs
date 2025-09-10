using System;
using System.Collections;
using UnityEngine;

public class MeleeAttack : Attack
{
    [Tooltip("The DamageRelay has to be put on the collider object, this is melee attack specific")]
    [SerializeField] private DamageRelay damager;
    [SerializeField] private Collider damageArea;
    
    private void Awake()
    {
        if (damager is not null) damager.DisableDamage();
    }

    public override IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null)
    {
        yield return base.ExecuteAttack(ctx, onFinished);
        
        ctx.FaceTarget();

        MeleeAttackData data = CurrentData as MeleeAttackData;

        if (data is null) throw new ArgumentNullException(nameof(data));
        
        if (!string.IsNullOrEmpty(data.animationTrigger))
            ctx.Animator?.SetTrigger(data.animationTrigger);

        if (ctx.Audio && data.sfx) ctx.Audio.PlayOneShot(data.sfx);

        yield return new WaitForSeconds(data.windup);
        
        if (damager is not null) damager.EnableDamage(data, ctx.Attacker);
        
        yield return new WaitForSeconds(data.hitboxActiveTime);
        
        if (damager is not null) damager.DisableDamage();
        
        yield return new WaitForSeconds(data.recovery);
        
        onFinished?.Invoke();
    }
}
