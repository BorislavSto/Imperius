using System;
using System.Collections;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] private Transform shootOrigin;

    public override IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null)
    {
        yield return base.ExecuteAttack(ctx, onFinished);
        
        ctx.FaceTarget();
        
        RangedAttackData data = CurrentData as RangedAttackData;
        
        if (data is null) throw new ArgumentNullException(nameof(data));
        
        yield return new WaitForSeconds(data.windup);

        if (data.projectilePrefab is not null && ctx.Target is not null)
        {
            Vector3 baseDir = (ctx.Target.transform.position - shootOrigin.position).normalized;

            for (int i = 0; i < data.projectileCount; i++)
            {
                Vector3 shootDir = baseDir;

                if (data.projectileCount > 1)
                {
                    float angle = data.spreadAngle * ((float)i / (data.projectileCount - 1) - 0.5f);
                    shootDir = Quaternion.AngleAxis(angle, Vector3.up) * baseDir;
                }

                GameObject proj = Instantiate(data.projectilePrefab, shootOrigin.position, 
                    Quaternion.LookRotation(shootDir));

                DamageRelay damageRelay = proj.GetComponent<DamageRelay>();
                damageRelay?.EnableDamage(data, ctx.Attacker.gameObject);

                Rigidbody rb = proj.GetComponent<Rigidbody>();
                if (rb is not null)
                {
                    rb.useGravity = false;
                    rb.linearVelocity = shootDir * data.projectileSpeed;
                }

                Destroy(proj, data.projectileLifetime);
            }
        }

        yield return new WaitForSeconds(data.recovery);

        onFinished?.Invoke();
    }
}
