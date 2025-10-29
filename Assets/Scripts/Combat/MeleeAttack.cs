using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    public class MeleeAttack : Attack
    {
        [Tooltip("The DamageRelay has to be put on the collider object, this is melee attack specific")]
        private DamageRelay damager;

        private Collider damageArea;
        private MeleeAttackData currentData;

        public MeleeAttack(AttackData data, DamageRelay damager, Collider damageArea) : base(data)
        {
            this.damager = damager;
            this.damageArea = damageArea;
            if (damager is not null) damager.DisableDamage();
            currentData = data as MeleeAttackData;
        }

        public override IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null)
        {
            ctx.FaceTarget();

            if (currentData is null) 
                throw new ArgumentNullException(nameof(currentData));

            if (!string.IsNullOrEmpty(currentData.animationTrigger))
                ctx.Animator?.SetTrigger(currentData.animationTrigger);

            if (ctx.Audio && currentData.sfx) 
                ctx.Audio.PlayOneShot(currentData.sfx);

            yield return new WaitForSeconds(currentData.windup);

            if (damager is not null)
                damager.EnableDamage(currentData, ctx.Attacker);

            yield return new WaitForSeconds(currentData.hitboxActiveTime);

            if (damager is not null)
                damager.DisableDamage();

            yield return new WaitForSeconds(currentData.recovery);

            onFinished?.Invoke();
        }
    }
}
