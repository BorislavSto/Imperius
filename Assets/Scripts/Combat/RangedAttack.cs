using System;
using System.Collections;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] private Transform shootOrigin;
    protected RangedAttackData currentData;

    public RangedAttack(AttackData data, Transform shootOrigin) : base(data)
    {
        this.shootOrigin = shootOrigin;
        currentData = data as RangedAttackData;
    }

    public override IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null)
    {
        ctx.FaceTarget();
        
        if (currentData is null) throw new ArgumentNullException(nameof(currentData));

        if (!string.IsNullOrEmpty(currentData.animationTrigger))
            ctx.Animator?.SetTrigger(currentData.animationTrigger);
        
        if (ctx.Audio && currentData.sfx) ctx.Audio.PlayOneShot(currentData.sfx);
        
        yield return new WaitForSeconds(currentData.windup);
        
        if (currentData.projectilePrefab is not null && ctx.Target is not null)
        {
            Vector3 baseDir = (ctx.Target.position - shootOrigin.position).normalized;

            for (int i = 0; i < currentData.projectileCount; i++)
            {
                Vector3 shootDir = baseDir;

                if (currentData.projectileCount > 1)
                {
                    float angle = currentData.spreadAngle * ((float)i / (currentData.projectileCount - 1) - 0.5f);
                    shootDir = Quaternion.AngleAxis(angle, Vector3.up) * baseDir;
                }

                GameObject proj = UnityEngine.Object.Instantiate(currentData.projectilePrefab, shootOrigin.position, 
                    Quaternion.LookRotation(shootDir));

                DamageRelay damageRelay = proj.GetComponent<DamageRelay>();
                damageRelay?.EnableDamage(currentData, ctx.Attacker.gameObject);

                Rigidbody rb = proj.GetComponent<Rigidbody>();
                if (rb is not null)
                {
                    rb.useGravity = false;
                    rb.linearVelocity = shootDir * currentData.projectileSpeed;
                }

                UnityEngine.Object.Destroy(proj, currentData.projectileLifetime);
            }
        }

        yield return new WaitForSeconds(currentData.recovery);

        onFinished?.Invoke();
    }
}
