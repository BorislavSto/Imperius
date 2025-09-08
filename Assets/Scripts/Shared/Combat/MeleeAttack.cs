using System;
using System.Collections;
using UnityEngine;

public class MeleeAttack : Attack
{
    [SerializeField] private Collider damageArea;

    private void Awake()
    {
        if (damageArea != null) damageArea.enabled = false;
    }

    public override IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null)
    {
        yield return base.ExecuteAttack(ctx, onFinished);
        
        ctx.FaceTarget();

        MeleeAttackData data = CurrentData as MeleeAttackData;
        
        if (data == null) throw new ArgumentNullException(nameof(data));

        if (!string.IsNullOrEmpty(data.animationTrigger))
            ctx.Animator?.SetTrigger(data.animationTrigger);

        if (ctx.Audio && data.sfx) ctx.Audio.PlayOneShot(data.sfx);

        yield return new WaitForSeconds(data.windup);
        if (damageArea) damageArea.enabled = true;
        yield return new WaitForSeconds(data.hitboxActiveTime);
        if (damageArea) damageArea.enabled = false;
        yield return new WaitForSeconds(data.recovery);
        
        onFinished?.Invoke();
    }
}
