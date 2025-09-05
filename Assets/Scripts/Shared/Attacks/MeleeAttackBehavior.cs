using System;
using System.Collections;
using UnityEngine;

public class MeleeAttackBehavior : IAttackBehavior
{
    private readonly MeleeAttackAsset data;
    private readonly MeleeAttack executor;

    public MeleeAttackBehavior(MeleeAttackAsset data, MeleeAttack executor)
    {
        this.data = data;
        this.executor = executor;
    }

    public IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null)
    {
        ctx.FaceTarget();

        if (!string.IsNullOrEmpty(data.animationTrigger))
            ctx.Animator?.SetTrigger(data.animationTrigger);

        if (ctx.Audio && data.sfx) ctx.Audio.PlayOneShot(data.sfx);

        yield return new WaitForSeconds(data.windup);
        if (executor.DamageArea) executor.DamageArea.enabled = true;
        yield return new WaitForSeconds(data.hitboxActiveTime);
        if (executor.DamageArea) executor.DamageArea.enabled = false;
        yield return new WaitForSeconds(data.recovery);
        
        onFinished?.Invoke();
    }
}
